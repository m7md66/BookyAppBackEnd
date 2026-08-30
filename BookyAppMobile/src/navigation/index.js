import React, { useEffect, useState } from 'react';
import { NavigationContainer, DefaultTheme } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useAuthStore } from '../store/authStore';
import { colors } from '../theme';
import AuthNavigator from './AuthNavigator';
import MainNavigator from './MainNavigator';
import InterestsScreen from '../screens/auth/InterestsScreen';

const Stack = createNativeStackNavigator();

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
  const { token, needsInterests, loadToken } = useAuthStore();
  const [ready, setReady] = useState(false);

  useEffect(() => {
    loadToken().finally(() => setReady(true));
  }, []);

  if (!ready) return null;

  return (
    <NavigationContainer theme={navTheme}>
      {!token ? <AuthNavigator /> : needsInterests ? <InterestsScreen /> : <MainNavigator />}
    </NavigationContainer>
  );
}
