import React, { useState, useCallback, useRef, useEffect } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl, TouchableOpacity, Modal, TextInput, KeyboardAvoidingView, Platform } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { useTranslation } from 'react-i18next';
import * as DocumentPicker from 'expo-document-picker';
import { getBrowseBooks, addBook, favorBook } from '../../api/books';
import { getAllInterests } from '../../api/interests';
import { uploadFile } from '../../api/files';
import BookCard from '../../components/BookCard';
import { colors, radius } from '../../theme';

export default function BrowseBooksScreen({ route, navigation }) {
  const { t } = useTranslation();
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [favoritingId, setFavoritingId] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [highlightedId, setHighlightedId] = useState(null);
  const listRef = useRef(null);

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

  useEffect(() => {
    const highlightBookId = route?.params?.highlightBookId;
    if (!highlightBookId || books.length === 0) return;

    const index = books.findIndex((b) => b.id === highlightBookId);
    if (index >= 0) {
      setHighlightedId(highlightBookId);
      listRef.current?.scrollToIndex({ index, animated: true, viewPosition: 0.2 });
    }
    navigation.setParams({ highlightBookId: undefined });
  }, [route?.params?.highlightBookId, books, navigation]);

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

  if (loading) return <View style={styles.center}><ActivityIndicator size="large" color={colors.primary} /></View>;

  return (
    <>
      <FlatList
        ref={listRef}
        data={books}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <BookCard
            book={item}
            onFavorite={() => handleFavorite(item)}
            favoriteBusy={favoritingId === item.id}
            highlighted={item.id === highlightedId}
          />
        )}
        onScrollToIndexFailed={({ index }) => {
          setTimeout(() => listRef.current?.scrollToIndex({ index, animated: true, viewPosition: 0.2 }), 100);
        }}
        contentContainerStyle={styles.list}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
        ListHeaderComponent={
          <View style={styles.headerRow}>
            <Text style={styles.header}>{t('browse.title')}</Text>
            <TouchableOpacity style={styles.addButton} onPress={() => setModalVisible(true)}>
              <Text style={styles.addButtonText}>{t('browse.addBook')}</Text>
            </TouchableOpacity>
          </View>
        }
        ListEmptyComponent={<Text style={styles.empty}>{t('browse.empty')}</Text>}
      />
      <AddBookModal visible={modalVisible} onClose={() => setModalVisible(false)} onAdded={handleAdded} />
    </>
  );
}

function AddBookModal({ visible, onClose, onAdded }) {
  const { t } = useTranslation();
  const [title, setTitle] = useState('');
  const [auther, setAuther] = useState('');
  const [description, setDescription] = useState('');
  const [bookFile, setBookFile] = useState(null);
  const [genres, setGenres] = useState([]);
  const [selectedGenreIds, setSelectedGenreIds] = useState([]);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!visible) return;
    getAllInterests().then((res) => setGenres(res.data.data ?? []));
  }, [visible]);

  const toggleGenre = (id) => {
    setSelectedGenreIds((prev) => prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]);
  };

  const reset = () => { setTitle(''); setAuther(''); setDescription(''); setBookFile(null); setSelectedGenreIds([]); setError(null); };

  const handlePickFile = async () => {
    const result = await DocumentPicker.getDocumentAsync({ type: 'application/pdf', copyToCacheDirectory: true });
    if (result.canceled) return;
    setBookFile(result.assets[0]);
  };

  const handleSubmit = async () => {
    if (!title.trim()) { setError(t('browse.titleRequired')); return; }
    setSubmitting(true);
    setError(null);
    try {
      let contentFileUrl = null;
      let coverImageUrl = null;
      if (bookFile) {
        const uploadRes = await uploadFile(bookFile);
        contentFileUrl = uploadRes.data.fileUrl;
        coverImageUrl = uploadRes.data.coverImageUrl;
      }
      const res = await addBook({
        Title: title.trim(),
        Auther: auther.trim(),
        Description: description.trim(),
        ContentFileUrl: contentFileUrl,
        CoverImageUrl: coverImageUrl,
        GenreIds: selectedGenreIds,
      });
      if (res.data?.status === false) {
        setError(res.data.errors?.[0] ?? t('browse.addFailed'));
        return;
      }
      reset();
      onAdded();
    } catch (e) {
      setError(e.response?.data?.errors?.[0] ?? t('browse.addFailed'));
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal visible={visible} animationType="slide" transparent onRequestClose={onClose}>
      <KeyboardAvoidingView style={styles.modalOverlay} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View style={styles.modalCard}>
          <Text style={styles.modalTitle}>{t('browse.modalTitle')}</Text>
          {error ? <Text style={styles.error}>{error}</Text> : null}
          <TextInput style={styles.input} placeholder={t('browse.titleField')} placeholderTextColor={colors.textSecondary} value={title} onChangeText={setTitle} />
          <TextInput style={styles.input} placeholder={t('browse.authorField')} placeholderTextColor={colors.textSecondary} value={auther} onChangeText={setAuther} />
          <TextInput style={[styles.input, styles.multiline]} placeholder={t('browse.descriptionField')} placeholderTextColor={colors.textSecondary} value={description} onChangeText={setDescription} multiline />
          {genres.length > 0 && (
            <>
              <Text style={styles.genresLabel}>{t('browse.genresLabel')}</Text>
              <View style={styles.genresRow}>
                {genres.map((genre) => {
                  const isSelected = selectedGenreIds.includes(genre.id);
                  return (
                    <TouchableOpacity
                      key={genre.id}
                      style={[styles.genreChip, isSelected && styles.genreChipSelected]}
                      onPress={() => toggleGenre(genre.id)}
                    >
                      <Text style={[styles.genreChipText, isSelected && styles.genreChipTextSelected]}>{genre.name}</Text>
                    </TouchableOpacity>
                  );
                })}
              </View>
            </>
          )}
          <TouchableOpacity style={styles.filePickerButton} onPress={handlePickFile}>
            <Text style={styles.filePickerText}>{bookFile ? bookFile.name : t('browse.chooseFile')}</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.submitButton} onPress={handleSubmit} disabled={submitting}>
            {submitting ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.submitButtonText}>{t('browse.submit')}</Text>}
          </TouchableOpacity>
          <TouchableOpacity onPress={() => { reset(); onClose(); }}>
            <Text style={styles.cancel}>{t('common.cancel')}</Text>
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
  header: { fontSize: 26, fontWeight: 'bold', color: colors.text },
  addButton: { backgroundColor: colors.primary, borderRadius: radius.sm, paddingVertical: 8, paddingHorizontal: 14 },
  addButtonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 14 },
  empty: { textAlign: 'center', color: colors.textSecondary, fontSize: 14, marginTop: 40 },
  modalOverlay: { flex: 1, justifyContent: 'flex-end', backgroundColor: colors.overlay },
  modalCard: { backgroundColor: colors.card, borderTopLeftRadius: radius.xl, borderTopRightRadius: radius.xl, padding: 24 },
  modalTitle: { fontSize: 20, fontWeight: 'bold', color: colors.text, marginBottom: 16 },
  input: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, fontSize: 15, marginBottom: 14, color: colors.text, backgroundColor: colors.card },
  multiline: { height: 90, textAlignVertical: 'top' },
  genresLabel: { fontSize: 13, fontWeight: '600', color: colors.textSecondary, marginBottom: 8 },
  genresRow: { flexDirection: 'row', flexWrap: 'wrap', gap: 8, marginBottom: 14 },
  genreChip: { paddingVertical: 8, paddingHorizontal: 14, borderRadius: radius.sm, borderWidth: 1, borderColor: colors.border, backgroundColor: colors.background },
  genreChipSelected: { backgroundColor: colors.primary, borderColor: colors.primary },
  genreChipText: { color: colors.textSecondary, fontWeight: '500', fontSize: 13 },
  genreChipTextSelected: { color: colors.onPrimary },
  filePickerButton: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, marginBottom: 14, borderStyle: 'dashed' },
  filePickerText: { fontSize: 14, color: colors.primary, textAlign: 'center' },
  submitButton: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 12 },
  submitButtonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
  cancel: { textAlign: 'center', color: colors.textSecondary, fontSize: 14, marginBottom: 8 },
  error: { color: colors.danger, marginBottom: 12, fontSize: 14 },
});
