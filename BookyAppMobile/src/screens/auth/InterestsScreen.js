import React, { useEffect, useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, FlatList, ActivityIndicator } from 'react-native';
import { getAllInterests, saveInterests } from '../../api/interests';
import { useAuthStore } from '../../store/authStore';

export default function InterestsScreen() {
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

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color="#4F46E5" /></View>;

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Your Interests</Text>
      <Text style={styles.subtitle}>Pick genres you love</Text>

      <FlatList
        data={interests}
        keyExtractor={(item) => item.id}
        numColumns={2}
        renderItem={({ item }) => {
          const isSelected = selected.includes(item.id);
          return (
            <TouchableOpacity
              style={[styles.chip, isSelected && styles.chipSelected]}
              onPress={() => toggle(item.id)}
            >
              <Text style={[styles.chipText, isSelected && styles.chipTextSelected]}>{item.name}</Text>
            </TouchableOpacity>
          );
        }}
        contentContainerStyle={styles.list}
      />

      <TouchableOpacity
        style={[styles.button, !selected.length && styles.buttonDisabled]}
        onPress={handleSave}
        disabled={!selected.length || saving}
      >
        {saving ? <ActivityIndicator color="#fff" /> : <Text style={styles.buttonText}>Let's Go</Text>}
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff', paddingHorizontal: 20, paddingTop: 60 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  title: { fontSize: 28, fontWeight: 'bold', color: '#1a1a1a', marginBottom: 4 },
  subtitle: { fontSize: 15, color: '#888', marginBottom: 24 },
  list: { paddingBottom: 24 },
  chip: { flex: 1, margin: 6, paddingVertical: 14, borderRadius: 12, borderWidth: 1.5, borderColor: '#ddd', alignItems: 'center' },
  chipSelected: { backgroundColor: '#4F46E5', borderColor: '#4F46E5' },
  chipText: { color: '#555', fontWeight: '500' },
  chipTextSelected: { color: '#fff' },
  button: { backgroundColor: '#4F46E5', borderRadius: 12, padding: 16, alignItems: 'center', marginBottom: 32 },
  buttonDisabled: { backgroundColor: '#a5b4fc' },
  buttonText: { color: '#fff', fontWeight: '600', fontSize: 16 },
});
