import { create } from 'zustand';
import { I18nManager } from 'react-native';
import { reloadAppAsync } from 'expo';
import AsyncStorage from '@react-native-async-storage/async-storage';
import i18n, { LANG_KEY, isRtlLanguage } from '../i18n';

export const useLanguageStore = create((set) => ({
  language: i18n.language || 'en',

  setLanguage: async (language) => {
    if (language === i18n.language) return;

    try {
      await AsyncStorage.setItem(LANG_KEY, language);
    } catch {
      // non-fatal
    }
    await i18n.changeLanguage(language);
    set({ language });

    const shouldBeRTL = isRtlLanguage(language);
    if (I18nManager.isRTL !== shouldBeRTL) {
      I18nManager.allowRTL(shouldBeRTL);
      I18nManager.forceRTL(shouldBeRTL);
      await reloadAppAsync();
    }
  },
}));
