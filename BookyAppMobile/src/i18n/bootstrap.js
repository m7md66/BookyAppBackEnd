import { I18nManager } from 'react-native';
import { reloadAppAsync } from 'expo';
import AsyncStorage from '@react-native-async-storage/async-storage';
import * as Localization from 'expo-localization';
import i18n, { LANG_KEY, SUPPORTED_LANGUAGES, isRtlLanguage } from './index';

function detectDeviceLanguage() {
  try {
    const code = (Localization.getLocales?.() ?? [])[0]?.languageCode ?? 'en';
    return code.toLowerCase().startsWith('ar') ? 'ar' : 'en';
  } catch {
    return 'en';
  }
}

async function readStoredLanguage() {
  try {
    const stored = await AsyncStorage.getItem(LANG_KEY);
    return SUPPORTED_LANGUAGES.includes(stored) ? stored : null;
  } catch {
    return null;
  }
}

/**
 * Resolves the active language and RTL direction before the first render.
 * Returns `{ reloading: true }` when a native reload was triggered to apply an
 * RTL direction change — in that case the caller should keep the splash up and
 * do nothing, because the app is about to restart.
 */
export async function bootstrapI18n() {
  let language = await readStoredLanguage();

  if (!language) {
    language = detectDeviceLanguage();
    // Persist immediately so the RTL reload below happens at most once.
    try {
      await AsyncStorage.setItem(LANG_KEY, language);
    } catch {
      // non-fatal: fall back to in-memory language only
    }
  }

  await i18n.changeLanguage(language);

  const shouldBeRTL = isRtlLanguage(language);
  if (I18nManager.isRTL !== shouldBeRTL) {
    I18nManager.allowRTL(shouldBeRTL);
    I18nManager.forceRTL(shouldBeRTL);
    await reloadAppAsync();
    return { reloading: true };
  }

  return { reloading: false };
}
