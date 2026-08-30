import React, { useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { useTranslation } from 'react-i18next';
import { getFavoriteBooks } from '../../api/books';
import BookCard from '../../components/BookCard';
import { colors } from '../../theme';

export default function MyLibraryScreen({ navigation }) {
  const { t } = useTranslation();
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const load = useCallback(async () => {
    try {
      const res = await getFavoriteBooks();
      setBooks(res.data.dataResult ?? []);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useFocusEffect(useCallback(() => { load(); }, [load]));

  const onRefresh = () => { setRefreshing(true); load(); };

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

  return (
    <FlatList
      data={books}
      keyExtractor={(item) => item.id}
      renderItem={({ item }) => <BookCard book={item} onPress={() => navigation.navigate('ReadBook', { book: item })} />}
      contentContainerStyle={styles.list}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
      ListHeaderComponent={<Text style={styles.header}>{t('library.title')}</Text>}
      ListEmptyComponent={<Text style={styles.empty}>{t('library.empty')}</Text>}
    />
  );
}

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  header: { fontSize: 26, fontWeight: 'bold', color: colors.text, marginBottom: 20 },
  empty: { textAlign: 'center', color: colors.textSecondary, fontSize: 14, marginTop: 40 },
});
