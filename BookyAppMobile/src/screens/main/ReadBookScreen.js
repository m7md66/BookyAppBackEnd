import React, { useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, ActivityIndicator, Alert, I18nManager } from 'react-native';
import { WebView } from 'react-native-webview';
import { useTranslation } from 'react-i18next';
import { createQuotation } from '../../api/quotations';
import { buildPdfViewerHtml } from './pdfViewerHtml';
import { colors } from '../../theme';

export default function ReadBookScreen({ route, navigation }) {
  const { t } = useTranslation();
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
        Alert.alert(t('readBook.posted'), t('readBook.postedBody'));
      } catch (e) {
        Alert.alert(t('readBook.error'), t('readBook.postFailed'));
      } finally {
        setPosting(false);
      }
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.backButton}>
          <Text style={styles.backArrow}>{I18nManager.isRTL ? '›' : '‹'} {t('common.back')}</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle} numberOfLines={1}>{book.title}</Text>
        <View style={styles.headerSpacer} />
      </View>

      {!book.contentFileUrl ? (
        <View style={styles.center}>
          <Text style={styles.emptyText}>{t('readBook.noFile')}</Text>
        </View>
      ) : (
        <WebView
          source={{ html: buildPdfViewerHtml(book.contentFileUrl, {
            prev: t('pdfViewer.prev'),
            next: t('pdfViewer.next'),
            loadingBook: t('pdfViewer.loadingBook'),
            postAsQuote: t('pdfViewer.postAsQuote'),
            failedToLoad: t('pdfViewer.failedToLoad'),
          }) }}
          onMessage={handleMessage}
          originWhitelist={['*']}
          javaScriptEnabled
          domStorageEnabled
          startInLoadingState
          renderLoading={() => (
            <View style={styles.center}>
              <ActivityIndicator size="large" color={colors.primary} />
            </View>
          )}
          style={styles.webview}
        />
      )}

      {posting && (
        <View style={styles.postingOverlay}>
          <ActivityIndicator color="#fff" />
          <Text style={styles.postingText}>{t('readBook.postingQuote')}</Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.card },
  header: {
    flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between',
    paddingTop: 48, paddingHorizontal: 12, paddingBottom: 10, borderBottomWidth: 1, borderBottomColor: colors.border,
  },
  backButton: { minWidth: 60 },
  backArrow: { color: colors.primary, fontSize: 15, fontWeight: '600' },
  headerTitle: { flex: 1, textAlign: 'center', fontSize: 15, fontWeight: '600', color: colors.text },
  headerSpacer: { minWidth: 60 },
  webview: { flex: 1 },
  center: { flex: 1, justifyContent: 'center', alignItems: 'center', padding: 24 },
  emptyText: { color: colors.textSecondary, fontSize: 14, textAlign: 'center' },
  postingOverlay: {
    position: 'absolute', bottom: 24, alignSelf: 'center', backgroundColor: 'rgba(0,0,0,0.75)',
    borderRadius: 10, paddingVertical: 10, paddingHorizontal: 16, flexDirection: 'row', alignItems: 'center', gap: 8,
  },
  postingText: { color: colors.onPrimary, fontSize: 13, marginStart: 8 },
});
