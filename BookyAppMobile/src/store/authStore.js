import { create } from 'zustand';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { login as loginApi, register as registerApi } from '../api/auth';

export const useAuthStore = create((set, get) => ({
  token: null,
  user: null,
  isLoading: false,
  error: null,
  needsInterests: false,

  login: async (email, password) => {
    set({ isLoading: true, error: null });
    try {
      const res = await loginApi({ email, password });
      const { token, ...user } = res.data.data ?? res.data;
      await AsyncStorage.setItem('token', token);
      set({ token, user, isLoading: false });
      return true;
    } catch (e) {
      set({ error: e.response?.data?.errors?.[0] ?? 'Login failed', isLoading: false });
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
        const msg = res.data.validationErrors?.[0]?.description ?? res.data.responseMessage ?? 'Register failed';
        set({ error: msg, isLoading: false });
        return false;
      }
      // AddUser doesn't return a token, so log in right after to authenticate the session
      const ok = await get().login(email, password);
      if (ok) set({ needsInterests: true });
      return ok;
    } catch (e) {
      set({ error: e.response?.data?.errors?.[0] ?? 'Register failed', isLoading: false });
      return false;
    }
  },

  completeInterests: () => set({ needsInterests: false }),

  logout: async () => {
    await AsyncStorage.removeItem('token');
    set({ token: null, user: null, needsInterests: false });
  },

  loadToken: async () => {
    const token = await AsyncStorage.getItem('token');
    if (token) set({ token });
  },
}));
