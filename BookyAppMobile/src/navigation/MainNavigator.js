import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { Ionicons } from '@expo/vector-icons';
import FeedScreen from '../screens/main/FeedScreen';
import BrowseBooksScreen from '../screens/main/BrowseBooksScreen';
import MyLibraryScreen from '../screens/main/MyLibraryScreen';
import ReadBookScreen from '../screens/main/ReadBookScreen';

const Tab = createBottomTabNavigator();
const Stack = createNativeStackNavigator();

const TAB_ICONS = {
  Feed: 'home',
  Browse: 'search',
  MyLibrary: 'library',
};

function MainTabs() {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        headerShown: false,
        tabBarIcon: ({ color, size, focused }) => (
          <Ionicons
            name={focused ? TAB_ICONS[route.name] : `${TAB_ICONS[route.name]}-outline`}
            size={size}
            color={color}
          />
        ),
      })}
    >
      <Tab.Screen name="Feed" component={FeedScreen} />
      <Tab.Screen name="Browse" component={BrowseBooksScreen} options={{ title: 'Browse Books' }} />
      <Tab.Screen name="MyLibrary" component={MyLibraryScreen} options={{ title: 'My Library' }} />
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
