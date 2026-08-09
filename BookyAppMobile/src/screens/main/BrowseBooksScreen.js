import React, { useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl, TouchableOpacity, Modal, TextInput, KeyboardAvoidingView, Platform } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import * as DocumentPicker from 'expo-document-picker';
import { getBrowseBooks, addBook, favorBook } from '../../api/books';
import { uploadFile } from '../../api/files';
import BookCard from '../../components/BookCard';

export default function BrowseBooksScreen() {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [favoritingId, setFavoritingId] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);

  const load = useCallback(async () => {
    try {
      const res = await getBrowseBooks();
      setBooks(res.data.dataResult ?? []);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useFocusEffect(useCallback(() => { load(); }, [load]));

  const onRefresh = () => { setRefreshing(true); load(); };

  const handleFavorite = async (book) => {
    setFavoritingId(book.id);
    setBooks((prev) => prev.map((b) => (b.id === book.id ? { ...b, isFavorite: true } : b)));
    try {
      await favorBook(book.id);
    } catch (e) {
      setBooks((prev) => prev.map((b) => (b.id === book.id ? { ...b, isFavorite: false } : b)));
    } finally {
      setFavoritingId(null);
    }
  };

  const handleAdded = () => {
    setModalVisible(false);
    load();
  };

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color="#4F46E5" /></View>;

  return (
    <>
      <FlatList
        data={books}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <BookCard book={item} onFavorite={() => handleFavorite(item)} favoriteBusy={favoritingId === item.id} />
        )}
        contentContainerStyle={styles.list}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
        ListHeaderComponent={
          <View style={styles.headerRow}>
            <Text style={styles.header}>Browse Books</Text>
            <TouchableOpacity style={styles.addButton} onPress={() => setModalVisible(true)}>
              <Text style={styles.addButtonText}>+ Add Book</Text>
            </TouchableOpacity>
          </View>
        }
        ListEmptyComponent={<Text style={styles.empty}>No books to browse right now.</Text>}
      />
      <AddBookModal visible={modalVisible} onClose={() => setModalVisible(false)} onAdded={handleAdded} />
    </>
  );
}

function AddBookModal({ visible, onClose, onAdded }) {
  const [title, setTitle] = useState('');
  const [auther, setAuther] = useState('');
  const [description, setDescription] = useState('');
  const [bookFile, setBookFile] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  const reset = () => { setTitle(''); setAuther(''); setDescription(''); setBookFile(null); setError(null); };

  const handlePickFile = async () => {
    const result = await DocumentPicker.getDocumentAsync({ type: 'application/pdf', copyToCacheDirectory: true });
    if (result.canceled) return;
    setBookFile(result.assets[0]);
  };

  const handleSubmit = async () => {
    if (!title.trim()) { setError('Title is required'); return; }
    setSubmitting(true);
    setError(null);
    try {
      let contentFileUrl = null;
      if (bookFile) {
        const uploadRes = await uploadFile(bookFile);
        contentFileUrl = uploadRes.data;
      }
      const res = await addBook({
        Title: title.trim(),
        Auther: auther.trim(),
        Description: description.trim(),
        ContentFileUrl: contentFileUrl,
      });
      if (res.data?.status === false) {
        setError(res.data.errors?.[0] ?? 'Failed to add book');
        return;
      }
      reset();
      onAdded();
    } catch (e) {
      setError(e.response?.data?.errors?.[0] ?? 'Failed to add book');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal visible={visible} animationType="slide" transparent onRequestClose={onClose}>
      <KeyboardAvoidingView style={styles.modalOverlay} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View style={styles.modalCard}>
          <Text style={styles.modalTitle}>Add a Book</Text>
          {error ? <Text style={styles.error}>{error}</Text> : null}
          <TextInput style={styles.input} placeholder="Title" placeholderTextColor="#888" value={title} onChangeText={setTitle} />
          <TextInput style={styles.input} placeholder="Author" placeholderTextColor="#888" value={auther} onChangeText={setAuther} />
          <TextInput style={[styles.input, styles.multiline]} placeholder="Description" placeholderTextColor="#888" value={description} onChangeText={setDescription} multiline />
          <TouchableOpacity style={styles.filePickerButton} onPress={handlePickFile}>
            <Text style={styles.filePickerText}>{bookFile ? bookFile.name : '📄 Choose Book File (PDF)'}</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.submitButton} onPress={handleSubmit} disabled={submitting}>
            {submitting ? <ActivityIndicator color="#fff" /> : <Text style={styles.submitButtonText}>Add Book</Text>}
          </TouchableOpacity>
          <TouchableOpacity onPress={() => { reset(); onClose(); }}>
            <Text style={styles.cancel}>Cancel</Text>
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}

const styles = StyleSheet.create({
  center: { flex: 1, justifyContent: 'center', alignItems: 'center' },
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  headerRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 20 },
  header: { fontSize: 26, fontWeight: 'bold', color: '#1a1a1a' },
  addButton: { backgroundColor: '#4F46E5', borderRadius: 8, paddingVertical: 8, paddingHorizontal: 14 },
  addButtonText: { color: '#fff', fontWeight: '600', fontSize: 14 },
  empty: { textAlign: 'center', color: '#888', fontSize: 14, marginTop: 40 },
  modalOverlay: { flex: 1, justifyContent: 'flex-end', backgroundColor: 'rgba(0,0,0,0.4)' },
  modalCard: { backgroundColor: '#fff', borderTopLeftRadius: 20, borderTopRightRadius: 20, padding: 24 },
  modalTitle: { fontSize: 20, fontWeight: 'bold', color: '#1a1a1a', marginBottom: 16 },
  input: { borderWidth: 1, borderColor: '#ddd', borderRadius: 10, padding: 14, fontSize: 15, marginBottom: 14, color: '#1a1a1a' },
  multiline: { height: 90, textAlignVertical: 'top' },
  filePickerButton: { borderWidth: 1, borderColor: '#ddd', borderRadius: 10, padding: 14, marginBottom: 14, borderStyle: 'dashed' },
  filePickerText: { fontSize: 14, color: '#4F46E5', textAlign: 'center' },
  submitButton: { backgroundColor: '#4F46E5', borderRadius: 10, padding: 16, alignItems: 'center', marginBottom: 12 },
  submitButtonText: { color: '#fff', fontWeight: '600', fontSize: 16 },
  cancel: { textAlign: 'center', color: '#888', fontSize: 14, marginBottom: 8 },
  error: { color: '#ef4444', marginBottom: 12, fontSize: 14 },
});
