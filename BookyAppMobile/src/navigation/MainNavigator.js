import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import FeedScreen from '../screens/main/FeedScreen';
import BrowseBooksScreen from '../screens/main/BrowseBooksScreen';
import MyLibraryScreen from '../screens/main/MyLibraryScreen';
import ReadBookScreen from '../screens/main/ReadBookScreen';

const Tab = createBottomTabNavigator();
const Stack = createNativeStackNavigator();

function MainTabs() {
  return (
    <Tab.Navigator screenOptions={{ headerShown: false }}>
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
