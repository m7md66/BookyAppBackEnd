import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

// Muted / earthy set that harmonises with the deep-green + cream palette.
const COLORS = [
  '#2F5D50', '#3E6B7A', '#7A5C3E', '#8A6D3B', '#6E4B4B',
  '#4A6B52', '#5B5170', '#7C6A4A', '#556B60', '#8A5A44',
];

function hashString(str) {
  let hash = 0;
  for (let i = 0; i < str.length; i++) {
    hash = (hash << 5) - hash + str.charCodeAt(i);
    hash |= 0;
  }
  return Math.abs(hash);
}

export default function BookCoverPlaceholder({ title, author, size = 56 }) {
  const titleInitial = title?.trim()?.[0]?.toUpperCase() ?? '';
  const authorInitial = author?.trim()?.[0]?.toUpperCase() ?? '';
  const color = COLORS[hashString(`${title ?? ''}${author ?? ''}`) % COLORS.length];

  return (
    <View style={[styles.cover, { width: size, height: size * 1.4, backgroundColor: color }]}>
      <Text style={[styles.initials, { fontSize: size * 0.36 }]}>{titleInitial}{authorInitial}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  cover: { borderRadius: 8, justifyContent: 'center', alignItems: 'center' },
  initials: { color: '#fff', fontWeight: '700' },
});
