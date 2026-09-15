import React, { useEffect, useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, ScrollView, ActivityIndicator } from 'react-native';
import { useTranslation } from 'react-i18next';
import { getAllInterests, saveInterests } from '../../api/interests';
import { useAuthStore } from '../../store/authStore';
import { colors, radius } from '../../theme';

export default function InterestsScreen() {
  const { t } = useTranslation();
  const [interests, setInterests] = useState([]);
  const [selected, setSelected] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const completeInterests = useAuthStore((state) => state.completeInterests);

  useEffect(() => {
    getAllInterests()
      .then((res) => setInterests(res.data.data ?? []))
      .finally(() => setLoading(false));
  }, []);

  const toggle = (id) => {
    setSelected((prev) => prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]);
  };

  const handleSave = async () => {
    if (!selected.length) return;
    setSaving(true);
    await saveInterests(selected);
    setSaving(false);
    completeInterests();
  };

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

  return (
    <View style={styles.container}>
      <Text style={styles.title}>{t('interests.title')}</Text>
      <Text style={styles.subtitle}>{t('interests.subtitle')}</Text>

      <ScrollView contentContainerStyle={styles.list}>
        <View style={styles.chipsWrap}>
          {interests.map((item) => {
            const isSelected = selected.includes(item.id);
            return (
              <TouchableOpacity
                key={item.id}
                style={[styles.chip, isSelected && styles.chipSelected]}
                onPress={() => toggle(item.id)}
              >
                <Text style={[styles.chipText, isSelected && styles.chipTextSelected]}>{item.name}</Text>
              </TouchableOpacity>
            );
          })}
        </View>
      </ScrollView>

      <TouchableOpacity
        style={[styles.button, !selected.length && styles.buttonDisabled]}
        onPress={handleSave}
        disabled={!selected.length || saving}
      >
        {saving ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('interests.cta')}</Text>}
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background, paddingHorizontal: 20, paddingTop: 60 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  title: { fontSize: 28, fontWeight: 'bold', color: colors.text, marginBottom: 4 },
  subtitle: { fontSize: 15, color: colors.textSecondary, marginBottom: 24 },
  list: { paddingBottom: 24 },
  chipsWrap: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  chip: { paddingHorizontal: 18, paddingVertical: 12, borderRadius: radius.md, borderWidth: 1.5, borderColor: colors.border, alignItems: 'center', backgroundColor: colors.card },
  chipSelected: { backgroundColor: colors.primary, borderColor: colors.primary },
  chipText: { color: colors.textTertiary, fontWeight: '500' },
  chipTextSelected: { color: colors.onPrimary },
  button: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 32 },
  buttonDisabled: { backgroundColor: colors.primaryDisabled },
  buttonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
});
