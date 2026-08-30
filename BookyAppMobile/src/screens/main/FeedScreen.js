import React, { useEffect, useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl } from 'react-native';
import { useTranslation } from 'react-i18next';
import { getFeed } from '../../api/quotations';
import QuotationCard from '../../components/QuotationCard';
import { colors } from '../../theme';

export default function FeedScreen() {
  const { t } = useTranslation();
  const [quotations, setQuotations] = useState([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const load = useCallback(async (pageNum = 1, replace = false) => {
    try {
      const res = await getFeed(pageNum, 20);
      const items = res.data.dataResult?.items ?? res.data.data ?? [];
      setQuotations((prev) => {
        if (replace) return items;
        const seen = new Set(prev.map((q) => q.id));
        return [...prev, ...items.filter((q) => !seen.has(q.id))];
      });
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

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

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
          <Text style={styles.header}>{t('feed.title')}</Text>
        </View>
      }
    />
  );
}

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  headerRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 },
  header: { fontSize: 26, fontWeight: 'bold', color: colors.text },
});
