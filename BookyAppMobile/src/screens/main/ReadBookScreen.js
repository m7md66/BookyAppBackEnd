import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, ActivityIndicator, Alert } from 'react-native';
import { WebView } from 'react-native-webview';
import { createQuotation } from '../../api/quotations';
import { buildPdfViewerHtml } from './pdfViewerHtml';

export default function ReadBookScreen({ route, navigation }) {
  const { book } = route.params;
  const [posting, setPosting] = useState(false);

  const handleMessage = async (event) => {
    let data;
    try {
      data = JSON.parse(event.nativeEvent.data);
    } catch {
      return;
    }
    if (data.type === 'quote' && data.text) {
      setPosting(true);
      try {
        await createQuotation(book.id, data.text);
        Alert.alert('Posted', 'Your quote was posted to the feed.');
      } catch (e) {
        Alert.alert('Error', 'Failed to post the quote. Please try again.');
      } finally {
        setPosting(false);
      }
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.backButton}>
          <Text style={styles.backArrow}>‹ Back</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle} numberOfLines={1}>{book.title}</Text>
        <View style={styles.headerSpacer} />
      </View>

      {!book.contentFileUrl ? (
        <View style={styles.center}>
          <Text style={styles.emptyText}>No file was uploaded for this book.</Text>
        </View>
      ) : (
        <WebView
          source={{ html: buildPdfViewerHtml(book.contentFileUrl) }}
          onMessage={handleMessage}
          originWhitelist={['*']}
          javaScriptEnabled
          domStorageEnabled
          startInLoadingState
          renderLoading={() => (
            <View style={styles.center}>
              <ActivityIndicator size="large" color="#4F46E5" />
            </View>
          )}
          style={styles.webview}
        />
      )}

      {posting && (
        <View style={styles.postingOverlay}>
          <ActivityIndicator color="#fff" />
          <Text style={styles.postingText}>Posting quote…</Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff' },
  header: {
    flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between',
    paddingTop: 48, paddingHorizontal: 12, paddingBottom: 10, borderBottomWidth: 1, borderBottomColor: '#eee',
  },
  backButton: { minWidth: 60 },
  backArrow: { color: '#4F46E5', fontSize: 15, fontWeight: '600' },
  headerTitle: { flex: 1, textAlign: 'center', fontSize: 15, fontWeight: '600', color: '#1a1a1a' },
  headerSpacer: { minWidth: 60 },
  webview: { flex: 1 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center', padding: 24 },
  emptyText: { color: '#888', fontSize: 14, textAlign: 'center' },
  postingOverlay: {
    position: 'absolute', bottom: 24, alignSelf: 'center', backgroundColor: 'rgba(0,0,0,0.75)',
    borderRadius: 10, paddingVertical: 10, paddingHorizontal: 16, flexDirection: 'row', alignItems: 'center', gap: 8,
  },
  postingText: { color: '#fff', fontSize: 13, marginLeft: 8 },
});
