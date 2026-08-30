import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { useTranslation } from 'react-i18next';
import BookCoverPlaceholder from './BookCoverPlaceholder';
import { colors, radius, shadow } from '../theme';

export default function BookCard({ book, onFavorite, favoriteBusy, onPress, highlighted }) {
  const { t } = useTranslation();
  const Container = onPress ? TouchableOpacity : View;
  return (
    <Container style={[styles.card, highlighted && styles.cardHighlighted]} onPress={onPress} activeOpacity={onPress ? 0.7 : 1}>
      <View style={styles.row}>
        <BookCoverPlaceholder title={book.title} author={book.auther} size={56} />
        <View style={styles.info}>
          <Text style={styles.title}>{book.title}</Text>
          {book.auther ? <Text style={styles.author}>{book.auther}</Text> : null}
          {book.description ? <Text style={styles.description} numberOfLines={3}>{book.description}</Text> : null}
        </View>
      </View>
      {onFavorite ? (
        <TouchableOpacity style={styles.favoriteButton} onPress={onFavorite} disabled={book.isFavorite || favoriteBusy}>
          <Text style={[styles.favoriteText, book.isFavorite && styles.favoriteTextActive]}>
            {book.isFavorite ? t('book.inLibrary') : t('book.addToLibrary')}
          </Text>
        </TouchableOpacity>
      ) : null}
    </Container>
  );
}

const styles = StyleSheet.create({
  card: { backgroundColor: colors.card, borderRadius: radius.lg, padding: 18, marginBottom: 14, ...shadow.card },
  cardHighlighted: { borderWidth: 2, borderColor: colors.accent },
  row: { flexDirection: 'row', gap: 14 },
  info: { flex: 1 },
  title: { fontSize: 17, fontWeight: '600', color: colors.text, marginBottom: 4 },
  author: { fontSize: 13, color: colors.textSecondary, marginBottom: 10 },
  description: { fontSize: 14, color: colors.textTertiary, lineHeight: 20 },
  favoriteButton: { marginTop: 14 },
  favoriteText: { fontSize: 14, fontWeight: '600', color: colors.primary },
  favoriteTextActive: { color: colors.textSecondary },
});
