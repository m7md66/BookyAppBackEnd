import React, { useEffect, useState } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useAuthStore } from '../store/authStore';
import AuthNavigator from './AuthNavigator';
import MainNavigator from './MainNavigator';
import InterestsScreen from '../screens/auth/InterestsScreen';

const Stack = createNativeStackNavigator();

export default function RootNavigator() {
  const { token, needsInterests, loadToken } = useAuthStore();
  const [ready, setReady] = useState(false);

  useEffect(() => {
    loadToken().finally(() => setReady(true));
  }, []);

  if (!ready) return null;

  return (
    <NavigationContainer>
      {!token ? <AuthNavigator /> : needsInterests ? <InterestsScreen /> : <MainNavigator />}
    </NavigationContainer>
  );
}
