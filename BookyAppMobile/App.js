import React, { useEffect, useState } from 'react';
import * as SplashScreen from 'expo-splash-screen';
import { StatusBar } from 'expo-status-bar';
import RootNavigator from './src/navigation';
import { bootstrapI18n } from './src/i18n/bootstrap';

SplashScreen.preventAutoHideAsync().catch(() => {});

export default function App() {
  const [ready, setReady] = useState(false);

  useEffect(() => {
    bootstrapI18n()
      .then(({ reloading }) => {
        if (!reloading) {
          setReady(true);
          SplashScreen.hideAsync().catch(() => {});
        }
        // when `reloading` is true the app restarts — keep the splash up
      })
      .catch(() => {
        setReady(true);
        SplashScreen.hideAsync().catch(() => {});
      });
  }, []);

  if (!ready) return null;

  return (
    <>
      <StatusBar style="dark" />
      <RootNavigator />
    </>
  );
}
