import React, { useEffect, useState } from 'react';
import { View, Text, ScrollView, StyleSheet, ActivityIndicator, Pressable } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { useTranslation } from 'react-i18next';
import { getQuotation } from '../../api/quotations';
import QuotationCard from '../../components/QuotationCard';
import { colors } from '../../theme';

// Landing screen for the "bookyapp://quotation/:id" deep link (opened from a shared /q/{id} web link).
export default function QuotationScreen({ route, navigation }) {
  const { t } = useTranslation();
  const { id } = route.params ?? {};
  const [quotation, setQuotation] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    let active = true;
    (async () => {
      try {
        const res = await getQuotation(id);
        const data = res.data?.data;
        if (!active) return;
        if (data) setQuotation(data);
        else setError(true);
      } catch {
        if (active) setError(true);
      } finally {
        if (active) setLoading(false);
      }
    })();
    return () => { active = false; };
  }, [id]);

  const goHome = () => {
    if (navigation.canGoBack()) navigation.goBack();
    else navigation.navigate('Tabs');
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Pressable onPress={goHome} hitSlop={12}>
          <Ionicons name="arrow-back" size={24} color={colors.text} />
        </Pressable>
        <Text style={styles.headerTitle}>{t('quotation.title')}</Text>
        <View style={{ width: 24 }} />
      </View>

      {loading ? (
        <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>
      ) : error || !quotation ? (
        <View style={styles.center}>
          <Text style={styles.errorText}>{t('quotation.notFound')}</Text>
          <Pressable style={styles.homeButton} onPress={goHome}>
            <Text style={styles.homeButtonText}>{t('common.back')}</Text>
          </Pressable>
        </View>
      ) : (
        <ScrollView contentContainerStyle={styles.body}>
          <QuotationCard quotation={quotation} />
        </ScrollView>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
    paddingTop: 52,
    paddingBottom: 12,
    backgroundColor: colors.card,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
  },
  headerTitle: { fontSize: 18, fontWeight: '600', color: colors.text },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center', padding: 24 },
  body: { padding: 16 },
  errorText: { fontSize: 15, color: colors.textSecondary, marginBottom: 16, textAlign: 'center' },
  homeButton: { backgroundColor: colors.primary, borderRadius: 10, paddingVertical: 12, paddingHorizontal: 24 },
  homeButtonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 15 },
});
