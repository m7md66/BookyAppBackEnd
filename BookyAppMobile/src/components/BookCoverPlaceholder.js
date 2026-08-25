import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const COLORS = [
  '#4F46E5', '#0EA5E9', '#059669', '#D97706', '#DC2626',
  '#7C3AED', '#DB2777', '#0891B2', '#65A30D', '#EA580C',
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
