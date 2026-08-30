import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { Ionicons } from '@expo/vector-icons';
import { useTranslation } from 'react-i18next';
import FeedScreen from '../screens/main/FeedScreen';
import BrowseBooksScreen from '../screens/main/BrowseBooksScreen';
import MyLibraryScreen from '../screens/main/MyLibraryScreen';
import ProfileScreen from '../screens/main/ProfileScreen';
import ReadBookScreen from '../screens/main/ReadBookScreen';
import { colors } from '../theme';

const Tab = createBottomTabNavigator();
const Stack = createNativeStackNavigator();

const TAB_ICONS = {
  Feed: 'home',
  Browse: 'search',
  MyLibrary: 'library',
  Profile: 'person',
};

function MainTabs() {
  const { t } = useTranslation();
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        headerShown: false,
        tabBarActiveTintColor: colors.primary,
        tabBarInactiveTintColor: colors.textSecondary,
        tabBarStyle: { backgroundColor: colors.card, borderTopColor: colors.border },
        tabBarIcon: ({ color, size, focused }) => (
          <Ionicons
            name={focused ? TAB_ICONS[route.name] : `${TAB_ICONS[route.name]}-outline`}
            size={size}
            color={color}
          />
        ),
      })}
    >
      <Tab.Screen name="Feed" component={FeedScreen} options={{ title: t('feed.title') }} />
      <Tab.Screen name="Browse" component={BrowseBooksScreen} options={{ title: t('browse.title') }} />
      <Tab.Screen name="MyLibrary" component={MyLibraryScreen} options={{ title: t('library.title') }} />
      <Tab.Screen name="Profile" component={ProfileScreen} options={{ title: t('profile.title') }} />
    </Tab.Navigator>
  );
}

export default function MainNavigator() {
  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      <Stack.Screen name="Tabs" component={MainTabs} />
      <Stack.Screen name="ReadBook" component={ReadBookScreen} />
    </Stack.Navigator>
  );
}
