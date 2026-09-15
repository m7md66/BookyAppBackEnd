import React, { useEffect, useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, ScrollView, ActivityIndicator, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { useTranslation } from 'react-i18next';
import { Ionicons } from '@expo/vector-icons';
import { getAllInterests, getUserInterests, saveInterests, addInterest } from '../../api/interests';
import { colors, radius } from '../../theme';

export default function EditInterestsScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation();
  const [interests, setInterests] = useState([]);
  const [selected, setSelected] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [adding, setAdding] = useState(false);
  const [newInterestName, setNewInterestName] = useState('');

  useEffect(() => {
    Promise.all([getAllInterests(), getUserInterests()])
      .then(([allRes, mineRes]) => {
        setInterests(allRes.data.data ?? []);
        setSelected((mineRes.data.data ?? []).map((item) => item.id));
      })
      .finally(() => setLoading(false));
  }, []);

  const toggle = (id) => {
    setSelected((prev) => prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]);
  };

  const handleAddInterest = async () => {
    const name = newInterestName.trim();
    if (!name) return;
    setAdding(true);
    try {
      const res = await addInterest(name);
      const created = res.data.data;
      if (created) {
        setInterests((prev) => prev.some((i) => i.id === created.id) ? prev : [...prev, created]);
        setSelected((prev) => prev.includes(created.id) ? prev : [...prev, created.id]);
        setNewInterestName('');
      }
    } catch {
      Alert.alert(t('interests.addFailed'));
    } finally {
      setAdding(false);
    }
  };

  const handleSave = async () => {
    if (!selected.length) return;
    setSaving(true);
    try {
      await saveInterests(selected);
      navigation.goBack();
    } catch {
      Alert.alert(t('interests.saveFailed'));
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.backButton}>
          <Ionicons name="chevron-back" size={24} color={colors.text} />
        </TouchableOpacity>
        <Text style={styles.title}>{t('interests.editTitle')}</Text>
      </View>
      <Text style={styles.subtitle}>{t('interests.editSubtitle')}</Text>

      <ScrollView style={styles.list} contentContainerStyle={styles.listContent}>
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

      <View style={styles.addRow}>
        <TextInput
          style={styles.addInput}
          placeholder={t('interests.addPlaceholder')}
          placeholderTextColor={colors.textTertiary}
          value={newInterestName}
          onChangeText={setNewInterestName}
        />
        <TouchableOpacity
          style={[styles.addButton, (!newInterestName.trim() || adding) && styles.buttonDisabled]}
          onPress={handleAddInterest}
          disabled={!newInterestName.trim() || adding}
        >
          {adding ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('interests.addButton')}</Text>}
        </TouchableOpacity>
      </View>

      <TouchableOpacity
        style={[styles.button, !selected.length && styles.buttonDisabled]}
        onPress={handleSave}
        disabled={!selected.length || saving}
      >
        {saving ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('interests.save')}</Text>}
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background, paddingHorizontal: 20, paddingTop: 60 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  header: { flexDirection: 'row', alignItems: 'center', marginBottom: 4 },
  backButton: { marginEnd: 8, marginStart: -8, padding: 4 },
  title: { fontSize: 28, fontWeight: 'bold', color: colors.text },
  subtitle: { fontSize: 15, color: colors.textSecondary, marginBottom: 24 },
  list: { flex: 1 },
  listContent: { paddingBottom: 24 },
  chipsWrap: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  chip: { paddingHorizontal: 18, paddingVertical: 12, borderRadius: radius.md, borderWidth: 1.5, borderColor: colors.border, alignItems: 'center', backgroundColor: colors.card },
  chipSelected: { backgroundColor: colors.primary, borderColor: colors.primary },
  chipText: { color: colors.textTertiary, fontWeight: '500' },
  chipTextSelected: { color: colors.onPrimary },
  addRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 16, gap: 8 },
  addInput: {
    flex: 1,
    borderWidth: 1.5,
    borderColor: colors.border,
    borderRadius: radius.md,
    paddingHorizontal: 14,
    paddingVertical: 12,
    color: colors.text,
    backgroundColor: colors.card,
  },
  addButton: { backgroundColor: colors.primary, borderRadius: radius.md, paddingHorizontal: 18, paddingVertical: 12, alignItems: 'center', justifyContent: 'center' },
  button: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 32 },
  buttonDisabled: { backgroundColor: colors.primaryDisabled },
  buttonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
});
