import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';
import { colors } from '../theme';

export default function Avatar({ imageUrl, name, size = 44 }) {
  const initial = name?.trim()?.[0]?.toUpperCase() ?? '?';
  const circle = { width: size, height: size, borderRadius: size / 2 };

  // Old/seeded accounts can carry a non-URL leftover in ImageUrl (e.g. an
  // email address); only treat it as a real avatar if it's an actual URL.
  const hasValidImage = /^https?:\/\//i.test(imageUrl ?? '');

  if (hasValidImage) {
    return <Image source={{ uri: imageUrl }} style={circle} />;
  }

  return (
    <View style={[circle, styles.placeholder]}>
      <Text style={[styles.initial, { fontSize: size * 0.4 }]}>{initial}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  placeholder: { backgroundColor: colors.primary, justifyContent: 'center', alignItems: 'center' },
  initial: { color: colors.onPrimary, fontWeight: 'bold' },
});
