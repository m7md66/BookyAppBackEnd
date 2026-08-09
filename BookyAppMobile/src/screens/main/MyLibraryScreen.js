import React, { useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { getFavoriteBooks } from '../../api/books';
import BookCard from '../../components/BookCard';

export default function MyLibraryScreen({ navigation }) {
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

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color="#4F46E5" /></View>;

  return (
    <FlatList
      data={books}
      keyExtractor={(item) => item.id}
      renderItem={({ item }) => <BookCard book={item} onPress={() => navigation.navigate('ReadBook', { book: item })} />}
      contentContainerStyle={styles.list}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
      ListHeaderComponent={<Text style={styles.header}>My Library</Text>}
      ListEmptyComponent={<Text style={styles.empty}>No books yet. Add some from Browse Books.</Text>}
    />
  );
}

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  header: { fontSize: 26, fontWeight: 'bold', color: '#1a1a1a', marginBottom: 20 },
  empty: { textAlign: 'center', color: '#888', fontSize: 14, marginTop: 40 },
});
