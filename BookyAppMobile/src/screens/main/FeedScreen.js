import React, { useEffect, useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl, TouchableOpacity } from 'react-native';
import { getFeed } from '../../api/quotations';
import QuotationCard from '../../components/QuotationCard';
import { useAuthStore } from '../../store/authStore';

export default function FeedScreen() {
  const logout = useAuthStore((state) => state.logout);
  const [quotations, setQuotations] = useState([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const load = useCallback(async (pageNum = 1, replace = false) => {
    try {
      const res = await getFeed(pageNum, 20);
      const items = res.data.dataResult?.items ?? res.data.data ?? [];
      setQuotations((prev) => replace ? items : [...prev, ...items]);
      setHasMore(items.length === 20);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useEffect(() => { load(1, true); }, []);

  const onRefresh = () => {
    setRefreshing(true);
    setPage(1);
    load(1, true);
  };

  const onEndReached = () => {
    if (!hasMore || loading) return;
    const next = page + 1;
    setPage(next);
    load(next);
  };

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color="#4F46E5" /></View>;

  return (
    <FlatList
      data={quotations}
      keyExtractor={(item) => item.id ?? Math.random().toString()}
      renderItem={({ item }) => <QuotationCard quotation={item} />}
      contentContainerStyle={styles.list}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
      onEndReached={onEndReached}
      onEndReachedThreshold={0.5}
      ListHeaderComponent={
        <View style={styles.headerRow}>
          <Text style={styles.header}>Feed</Text>
          <TouchableOpacity onPress={logout}>
            <Text style={styles.logout}>Logout</Text>
          </TouchableOpacity>
        </View>
      }
    />
  );
}

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  headerRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 },
  header: { fontSize: 26, fontWeight: 'bold', color: '#1a1a1a' },
  logout: { fontSize: 14, fontWeight: '600', color: '#4F46E5' },
});
