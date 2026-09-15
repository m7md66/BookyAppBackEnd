import React, { useState, useEffect, useCallback } from 'react';
import {
  View,
  Text,
  Modal,
  FlatList,
  TextInput,
  TouchableOpacity,
  KeyboardAvoidingView,
  Platform,
  ActivityIndicator,
  StyleSheet,
} from 'react-native';
import { useTranslation } from 'react-i18next';
import { getComments, commentQuotation } from '../api/quotations';
import Avatar from './Avatar';
import { colors, radius } from '../theme';

// The backend serializes CreatedDate (stored as UTC) without a timezone
// suffix, so a bare `new Date(dateStr)` would be misread as local time.
// Treat it as UTC unless it already carries an offset/Z.
function parseServerDate(dateStr) {
  const hasTimezone = /Z$|[+-]\d{2}:\d{2}$/.test(dateStr);
  return new Date(hasTimezone ? dateStr : `${dateStr}Z`);
}

function formatRelativeTime(dateStr, t) {
  if (!dateStr) return '';
  const diffMs = Date.now() - parseServerDate(dateStr).getTime();
  const diffMin = Math.floor(diffMs / 60000);
  if (diffMin < 1) return t('comments.justNow');
  if (diffMin < 60) return t('comments.minutesAgo', { count: diffMin });
  const diffHour = Math.floor(diffMin / 60);
  if (diffHour < 24) return t('comments.hoursAgo', { count: diffHour });
  const diffDay = Math.floor(diffHour / 24);
  return t('comments.daysAgo', { count: diffDay });
}

function CommentItem({ item }) {
  const { t } = useTranslation();
  const fullName = `${item.user?.firstName ?? ''} ${item.user?.lastName ?? ''}`.trim();
  return (
    <View style={styles.commentRow}>
      <Avatar imageUrl={item.user?.imageUrl} name={fullName} size={34} />
      <View style={styles.commentBubble}>
        <Text style={styles.commentAuthor}>{fullName}</Text>
        <Text style={styles.commentContent}>{item.content}</Text>
        {item.createdDate ? <Text style={styles.commentTime}>{formatRelativeTime(item.createdDate, t)}</Text> : null}
      </View>
    </View>
  );
}

export default function CommentsModal({ visible, quotationId, onClose, onCommentPosted }) {
  const { t } = useTranslation();
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);
  const [text, setText] = useState('');
  const [posting, setPosting] = useState(false);

  const loadComments = useCallback(async () => {
    if (!quotationId) return;
    setLoading(true);
    setError(false);
    try {
      const res = await getComments(quotationId, 1, 50);
      setComments(res.data?.dataResult?.items ?? []);
    } catch {
      setError(true);
    } finally {
      setLoading(false);
    }
  }, [quotationId]);

  useEffect(() => {
    if (visible) loadComments();
  }, [visible, loadComments]);

  const handlePost = async () => {
    const trimmed = text.trim();
    if (!trimmed || posting) return;
    setPosting(true);
    try {
      await commentQuotation({ comment: trimmed, QuotationId: quotationId });
      setText('');
      onCommentPosted?.();
      await loadComments();
    } finally {
      setPosting(false);
    }
  };

  return (
    <Modal visible={visible} animationType="slide" transparent onRequestClose={onClose}>
      <KeyboardAvoidingView style={styles.overlay} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View style={styles.sheet}>
          <View style={styles.header}>
            <Text style={styles.title}>{t('comments.title')}</Text>
            <TouchableOpacity onPress={onClose}>
              <Text style={styles.close}>✕</Text>
            </TouchableOpacity>
          </View>

          {loading ? (
            <ActivityIndicator style={styles.loading} color={colors.primary} />
          ) : error ? (
            <Text style={styles.empty}>{t('comments.loadFailed')}</Text>
          ) : (
            <FlatList
              data={comments}
              keyExtractor={(item, index) => item.id ?? String(index)}
              renderItem={({ item }) => <CommentItem item={item} />}
              contentContainerStyle={comments.length === 0 ? styles.emptyContainer : styles.listContent}
              ListEmptyComponent={<Text style={styles.empty}>{t('comments.empty')}</Text>}
            />
          )}

          <View style={styles.inputRow}>
            <TextInput
              style={styles.input}
              placeholder={t('comments.placeholder')}
              placeholderTextColor={colors.textSecondary}
              value={text}
              onChangeText={setText}
              multiline
            />
            <TouchableOpacity style={styles.postButton} onPress={handlePost} disabled={posting || !text.trim()}>
              {posting ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.postButtonText}>{t('comments.post')}</Text>}
            </TouchableOpacity>
          </View>
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}

const styles = StyleSheet.create({
  overlay: { flex: 1, justifyContent: 'flex-end', backgroundColor: colors.overlay },
  sheet: { height: '80%', backgroundColor: colors.card, borderTopLeftRadius: radius.xl, borderTopRightRadius: radius.xl, padding: 20 },
  header: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  title: { fontSize: 18, fontWeight: 'bold', color: colors.text },
  close: { fontSize: 18, color: colors.textSecondary, padding: 4 },
  loading: { marginTop: 24 },
  listContent: { paddingBottom: 12 },
  emptyContainer: { flexGrow: 1, justifyContent: 'center' },
  empty: { textAlign: 'center', color: colors.textSecondary, fontSize: 14 },
  commentRow: { flexDirection: 'row', gap: 10, marginBottom: 16 },
  commentBubble: { flex: 1, backgroundColor: colors.background, borderRadius: radius.md, padding: 10 },
  commentAuthor: { fontSize: 13, fontWeight: '600', color: colors.text, marginBottom: 2 },
  commentContent: { fontSize: 14, color: colors.text, lineHeight: 20 },
  commentTime: { fontSize: 11, color: colors.textSecondary, marginTop: 4 },
  inputRow: { flexDirection: 'row', alignItems: 'flex-end', gap: 10, paddingTop: 10, borderTopWidth: 1, borderTopColor: colors.border },
  input: { flex: 1, borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 10, fontSize: 14, color: colors.text, maxHeight: 90, backgroundColor: colors.card },
  postButton: { backgroundColor: colors.primary, borderRadius: radius.md, paddingHorizontal: 16, paddingVertical: 10, justifyContent: 'center' },
  postButtonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 14 },
});
