import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';

export default function BookCard({ book, onFavorite, favoriteBusy, onPress, highlighted }) {
  const Container = onPress ? TouchableOpacity : View;
  return (
    <Container style={[styles.card, highlighted && styles.cardHighlighted]} onPress={onPress} activeOpacity={onPress ? 0.7 : 1}>
      <Text style={styles.title}>{book.title}</Text>
      {book.auther ? <Text style={styles.author}>{book.auther}</Text> : null}
      {book.description ? <Text style={styles.description} numberOfLines={3}>{book.description}</Text> : null}
      {onFavorite ? (
        <TouchableOpacity style={styles.favoriteButton} onPress={onFavorite} disabled={book.isFavorite || favoriteBusy}>
          <Text style={[styles.favoriteText, book.isFavorite && styles.favoriteTextActive]}>
            {book.isFavorite ? '♥ In Library' : '♡ Add to Library'}
          </Text>
        </TouchableOpacity>
      ) : null}
    </Container>
  );
}

const styles = StyleSheet.create({
  card: { backgroundColor: '#fff', borderRadius: 14, padding: 18, marginBottom: 14, shadowColor: '#000', shadowOpacity: 0.06, shadowRadius: 8, shadowOffset: { width: 0, height: 2 }, elevation: 3 },
  cardHighlighted: { borderWidth: 2, borderColor: '#4F46E5' },
  title: { fontSize: 17, fontWeight: '600', color: '#1a1a1a', marginBottom: 4 },
  author: { fontSize: 13, color: '#888', marginBottom: 10 },
  description: { fontSize: 14, color: '#444', lineHeight: 20 },
  favoriteButton: { marginTop: 14 },
  favoriteText: { fontSize: 14, fontWeight: '600', color: '#4F46E5' },
  favoriteTextActive: { color: '#888' },
});
