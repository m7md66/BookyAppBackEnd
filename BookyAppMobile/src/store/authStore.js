import { create } from 'zustand';
import AsyncStorage from '@react-native-async-storage/async-storage';
import i18n from '../i18n';
import {
  login as loginApi,
  register as registerApi,
  confirmEmail as confirmEmailApi,
  resendOtp as resendOtpApi,
  getMyProfile as getMyProfileApi,
} from '../api/auth';

export const useAuthStore = create((set, get) => ({
  token: null,
  user: null,
  isLoading: false,
  error: null,
  needsInterests: false,
  // Set only right after a registration when the backend requires email confirmation
  // (Features:RequireEmailConfirmation). Stays false in dev, so the flow is unchanged there.
  needsEmailVerification: false,
  pendingEmail: null,

  login: async (email, password) => {
    set({ isLoading: true, error: null });
    try {
      const res = await loginApi({ email, password });
      const { token, ...user } = res.data.data ?? res.data;
      await AsyncStorage.setItem('token', token);
      set({ token, user, isLoading: false });
      return true;
    } catch (e) {
      set({ error: e.response?.data?.errors?.[0] ?? i18n.t('auth.loginFailed'), isLoading: false });
      return false;
    }
  },

  register: async (firstName, lastName, email, password) => {
    set({ isLoading: true, error: null });
    try {
      const res = await registerApi({ firstName, lastName, email, password });
      // AddUser always responds 200, even on failure (weak password, duplicate email, etc.) -
      // the real result is in isSuccess, so check it before treating registration as done
      if (res.data?.isSuccess === false) {
        const msg = res.data.validationErrors?.[0]?.description ?? res.data.responseMessage ?? i18n.t('auth.registerFailed');
        set({ error: msg, isLoading: false });
        return false;
      }
      // Production gates new accounts behind an emailed OTP code - show the verify screen
      // instead of logging in. In dev this flag is false and the flow is untouched.
      if (res.data?.requiresEmailConfirmation === true) {
        set({ needsEmailVerification: true, pendingEmail: email, isLoading: false });
        return true;
      }
      // AddUser doesn't return a token, so log in right after to authenticate the session
      const ok = await get().login(email, password);
      if (ok) set({ needsInterests: true });
      return ok;
    } catch (e) {
      set({ error: e.response?.data?.errors?.[0] ?? i18n.t('auth.registerFailed'), isLoading: false });
      return false;
    }
  },

  // Confirm the emailed OTP. On success the backend returns a fresh token, so we
  // authenticate the session and send the user on to the interests picker.
  verifyEmail: async (code) => {
    set({ isLoading: true, error: null });
    try {
      const res = await confirmEmailApi({ email: get().pendingEmail, code });
      const body = res.data?.data ?? res.data;
      if (body?.isSuccess === false || !body?.token) {
        set({ error: body?.responseMessage ?? i18n.t('auth.verifyEmail.failed'), isLoading: false });
        return false;
      }
      await AsyncStorage.setItem('token', body.token);
      set({ token: body.token, needsEmailVerification: false, pendingEmail: null, needsInterests: true });
      try {
        const me = await getMyProfileApi();
        set({ user: me.data?.data ?? me.data ?? { email: body.email } });
      } catch {
        set({ user: { email: body.email } });
      }
      set({ isLoading: false });
      return true;
    } catch (e) {
      set({ error: e.response?.data?.errors?.[0] ?? i18n.t('auth.verifyEmail.failed'), isLoading: false });
      return false;
    }
  },

  resendCode: async () => {
    try {
      const res = await resendOtpApi({ email: get().pendingEmail });
      if (res.data?.isSuccess === false) {
        return { ok: false, message: res.data.responseMessage ?? i18n.t('common.genericError') };
      }
      return { ok: true, message: i18n.t('auth.verifyEmail.sent') };
    } catch (e) {
      return { ok: false, message: e.response?.data?.errors?.[0] ?? i18n.t('common.genericError') };
    }
  },

  cancelEmailVerification: () => set({ needsEmailVerification: false, pendingEmail: null, error: null }),

  completeInterests: () => set({ needsInterests: false }),

  logout: async () => {
    await AsyncStorage.removeItem('token');
    set({ token: null, user: null, needsInterests: false, needsEmailVerification: false, pendingEmail: null });
  },

  loadToken: async () => {
    const token = await AsyncStorage.getItem('token');
    if (token) set({ token });
  },
}));
