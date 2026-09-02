import React, { useEffect, useState } from 'react';
import { NavigationContainer, DefaultTheme } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useAuthStore } from '../store/authStore';
import { SHARE_BASE_URL } from '../api/client';
import { colors } from '../theme';
import AuthNavigator from './AuthNavigator';
import MainNavigator from './MainNavigator';
import InterestsScreen from '../screens/auth/InterestsScreen';
import VerifyEmailScreen from '../screens/auth/VerifyEmailScreen';

const Stack = createNativeStackNavigator();

// Deep links: "bookyapp://quotation/:id" (from the shared /q/{id} web page) and,
// if universal links are configured later, "https://<domain>/q/:id".
// Only resolves while the authenticated MainNavigator (which owns the Quotation screen) is mounted.
const linking = {
  prefixes: ['bookyapp://', `${SHARE_BASE_URL}/`],
  config: {
    screens: {
      Quotation: 'quotation/:id',
    },
  },
};

const navTheme = {
  ...DefaultTheme,
  colors: {
    ...DefaultTheme.colors,
    primary: colors.primary,
    background: colors.background,
    card: colors.card,
    text: colors.text,
    border: colors.border,
    notification: colors.accent,
  },
};

export default function RootNavigator() {
  const { token, needsInterests, needsEmailVerification, loadToken } = useAuthStore();
  const [ready, setReady] = useState(false);

  useEffect(() => {
    loadToken().finally(() => setReady(true));
  }, []);

  if (!ready) return null;

  return (
    <NavigationContainer theme={navTheme} linking={linking}>
      {needsEmailVerification ? (
        <VerifyEmailScreen />
      ) : !token ? (
        <AuthNavigator />
      ) : needsInterests ? (
        <InterestsScreen />
      ) : (
        <MainNavigator />
      )}
    </NavigationContainer>
  );
}
