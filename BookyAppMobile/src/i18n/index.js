import { I18nManager } from 'react-native';
import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import en from './locales/en.json';
import ar from './locales/ar.json';

export const LANG_KEY = 'appLanguage';
export const SUPPORTED_LANGUAGES = ['en', 'ar'];
export const RTL_LANGUAGES = ['ar'];

// Synchronous init so `i18n` is usable at import time. The bootstrap module
// (src/i18n/bootstrap.js) switches to the resolved language before first render.
i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    ar: { translation: ar },
  },
  lng: 'en',
  fallbackLng: 'en',
  compatibilityJSON: 'v4',
  interpolation: { escapeValue: false },
  returnNull: false,
  react: { useSuspense: false },
});

export const isRTL = () => I18nManager.isRTL;
export const isRtlLanguage = (lng) => RTL_LANGUAGES.includes(lng);

export default i18n;
