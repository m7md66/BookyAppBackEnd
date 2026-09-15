import React, { useState } from 'react';
import { View, Text, Pressable, StyleSheet, Modal, TextInput, KeyboardAvoidingView, Platform, TouchableOpacity, ActivityIndicator, Share } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { useTranslation } from 'react-i18next';
import { likeQuotation, requoteQuotation, shareQuotation } from '../api/quotations';
import { SHARE_BASE_URL } from '../api/client';
import { colors, radius, shadow } from '../theme';
import CommentsModal from './CommentsModal';

function ActionButton({ label, onPress, children }) {
  const [hovered, setHovered] = useState(false);

  return (
    <View style={styles.actionWrapper}>
      {hovered && (
        <View style={styles.tooltip} pointerEvents="none">
          <Text style={styles.tooltipText}>{label}</Text>
          <View style={styles.tooltipArrow} />
        </View>
      )}
      <Pressable
        style={styles.action}
        onPress={onPress}
        onHoverIn={() => setHovered(true)}
        onHoverOut={() => setHovered(false)}
      >
        {children}
      </Pressable>
    </View>
  );
}

function QuotePreview({ quotation, onPress }) {
  const Wrapper = onPress ? Pressable : View;
  return (
    <View>
      <Text style={styles.content}>"{quotation.content}"</Text>
      <Wrapper style={styles.bookRow} onPress={onPress}>
        <Text style={styles.bookTitle}>{quotation.bookTitle}</Text>
        {quotation.bookAuther ? <Text style={styles.author}> · {quotation.bookAuther}</Text> : null}
      </Wrapper>
    </View>
  );
}

function RequoteModal({ visible, quotation, onClose, onSubmit }) {
  const { t } = useTranslation();
  const [comment, setComment] = useState('');
  const [submitting, setSubmitting] = useState(false);

  const handleSubmit = async () => {
    setSubmitting(true);
    try {
      await onSubmit(comment.trim());
      setComment('');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal visible={visible} animationType="slide" transparent onRequestClose={onClose}>
      <KeyboardAvoidingView style={styles.modalOverlay} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View style={styles.modalCard}>
          <Text style={styles.modalTitle}>{t('quotation.requote')}</Text>
          <View style={styles.previewCard}>
            <QuotePreview quotation={quotation} />
          </View>
          <TextInput
            style={[styles.input, styles.multiline]}
            placeholder={t('quotation.addComment')}
            placeholderTextColor={colors.textSecondary}
            value={comment}
            onChangeText={setComment}
            multiline
          />
          <TouchableOpacity style={styles.submitButton} onPress={handleSubmit} disabled={submitting}>
            {submitting ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.submitButtonText}>{t('quotation.requote')}</Text>}
          </TouchableOpacity>
          <TouchableOpacity onPress={onClose} disabled={submitting}>
            <Text style={styles.cancel}>{t('common.cancel')}</Text>
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}

export default function QuotationCard({ quotation }) {
  const { t } = useTranslation();
  const navigation = useNavigation();
  const [likes, setLikes] = useState(quotation.likesNumber ?? 0);
  const [requotes, setRequotes] = useState(quotation.reQueteNumber ?? 0);
  const [shares, setShares] = useState(quotation.sharesNumber ?? 0);
  const [liked, setLiked] = useState(quotation.isLikedByMe ?? false);
  const [requoted, setRequoted] = useState(quotation.isRequotedByMe ?? false);
  const [shared, setShared] = useState(quotation.isSharedByMe ?? false);
  const [modalVisible, setModalVisible] = useState(false);
  const [commentsVisible, setCommentsVisible] = useState(false);
  const [comments, setComments] = useState(quotation.commentsNumber ?? 0);

  const targetQuotationId = quotation.originalQuotationId ?? quotation.id;

  const handleLike = async () => {
    setLiked((v) => !v);
    setLikes((v) => liked ? v - 1 : v + 1);
    await likeQuotation(targetQuotationId);
  };

  const handleRequoteSubmit = async (comment) => {
    setRequoted((v) => !v);
    setRequotes((v) => requoted ? v - 1 : v + 1);
    await requoteQuotation(targetQuotationId, comment);
    setModalVisible(false);
  };

  const handleShare = async () => {
    const author = quotation.bookAuther ? `، ${quotation.bookAuther}` : '';
    const link = `${SHARE_BASE_URL}/q/${targetQuotationId}`;
    let result;
    try {
      result = await Share.share({
        message: `"${quotation.content}"\n\n— ${quotation.bookTitle}${author}\n\n${link}`,
      });
    } catch {
      return;
    }
    if (result.action === Share.dismissedAction) return;

    // Backend toggles the share record; only register the first time so the
    // counter reflects the number of distinct users who shared this quote.
    if (shared) return;

    setShared(true);
    setShares((v) => v + 1);
    try {
      const res = await shareQuotation(targetQuotationId);
      const isSharedNow = res?.data?.data;
      if (typeof isSharedNow === 'boolean' && !isSharedNow) {
        setShared(false);
        setShares((v) => Math.max(0, v - 1));
      }
    } catch {
      setShared(false);
      setShares((v) => Math.max(0, v - 1));
    }
  };

  const handleBookPress = () => {
    navigation.navigate('Browse', { highlightBookId: quotation.bookId });
  };

  return (
    <View style={styles.card}>
      {quotation.isRequote ? (
        <View style={styles.requoteHeader}>
          <Text style={styles.requoteHeaderIcon}>↺</Text>
          <Text style={styles.requoteHeaderText}>{t('quotation.reQuotedBy', { name: quotation.requoterFullName })}</Text>
        </View>
      ) : null}
      {quotation.isRequote && quotation.requoteComment ? (
        <Text style={styles.requoteComment}>{quotation.requoteComment}</Text>
      ) : null}

      <View style={quotation.isRequote ? styles.nestedCard : undefined}>
        {quotation.userFullName ? <Text style={styles.userName}>{quotation.userFullName}</Text> : null}
        <QuotePreview quotation={quotation} onPress={handleBookPress} />
      </View>

      <View style={styles.actions}>
        <ActionButton label={t('quotation.like')} onPress={handleLike}>
          <Text style={[styles.actionIcon, liked && styles.active]}>♥</Text>
          <Text style={styles.actionCount}>{likes}</Text>
        </ActionButton>
        <ActionButton label={t('quotation.requote')} onPress={() => setModalVisible(true)}>
          <Text style={[styles.actionIcon, requoted && styles.activeRequote]}>↺</Text>
          <Text style={styles.actionCount}>{requotes}</Text>
        </ActionButton>
        <ActionButton label={t('quotation.share')} onPress={handleShare}>
          <Text style={[styles.actionIcon, shared && styles.activeShare]}>↗</Text>
          <Text style={[styles.actionCount, shared && styles.activeShare]}>{shares}</Text>
        </ActionButton>
        <ActionButton label={t('quotation.comment')} onPress={() => setCommentsVisible(true)}>
          <Text style={styles.actionIcon}>💬</Text>
          <Text style={styles.actionCount}>{comments}</Text>
        </ActionButton>
      </View>

      <RequoteModal
        visible={modalVisible}
        quotation={quotation}
        onClose={() => setModalVisible(false)}
        onSubmit={handleRequoteSubmit}
      />

      <CommentsModal
        visible={commentsVisible}
        quotationId={targetQuotationId}
        onClose={() => setCommentsVisible(false)}
        onCommentPosted={() => setComments((v) => v + 1)}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  card: { backgroundColor: colors.card, borderRadius: radius.lg, padding: 18, marginBottom: 14, ...shadow.card },
  userName: { fontSize: 13, fontWeight: '600', color: colors.text, marginBottom: 6 },
  content: { fontSize: 16, color: colors.text, lineHeight: 24, marginBottom: 12, fontStyle: 'italic' },
  bookRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 14 },
  bookTitle: { fontSize: 13, fontWeight: '600', color: colors.primary },
  author: { fontSize: 13, color: colors.textSecondary },
  actions: { flexDirection: 'row', gap: 20 },
  actionWrapper: { alignItems: 'center', position: 'relative' },
  action: { flexDirection: 'row', alignItems: 'center', gap: 4 },
  actionIcon: { fontSize: 18, color: colors.textSecondary },
  actionCount: { fontSize: 13, color: colors.textSecondary },
  active: { color: colors.danger },
  activeRequote: { color: colors.primary },
  activeShare: { color: colors.accent },
  requoteHeader: { flexDirection: 'row', alignItems: 'center', gap: 6, marginBottom: 8 },
  requoteHeaderIcon: { fontSize: 13, color: colors.textSecondary },
  requoteHeaderText: { fontSize: 13, fontWeight: '600', color: colors.textSecondary },
  requoteComment: { fontSize: 15, color: colors.text, lineHeight: 22, marginBottom: 12 },
  nestedCard: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14 },
  tooltip: {
    position: 'absolute',
    bottom: '100%',
    marginBottom: 10,
    backgroundColor: colors.card,
    paddingHorizontal: 10,
    paddingVertical: 6,
    borderRadius: 6,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOpacity: 0.2,
    shadowRadius: 4,
    shadowOffset: { width: 0, height: 2 },
    elevation: 6,
    zIndex: 10,
  },
  tooltipText: { fontSize: 12, color: colors.text, fontWeight: '500' },
  tooltipArrow: {
    position: 'absolute',
    bottom: -4,
    width: 8,
    height: 8,
    backgroundColor: colors.card,
    transform: [{ rotate: '45deg' }],
  },
  modalOverlay: { flex: 1, justifyContent: 'flex-end', backgroundColor: colors.overlay },
  modalCard: { backgroundColor: colors.card, borderTopLeftRadius: radius.xl, borderTopRightRadius: radius.xl, padding: 24 },
  modalTitle: { fontSize: 20, fontWeight: 'bold', color: colors.text, marginBottom: 16 },
  previewCard: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, marginBottom: 14 },
  input: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, fontSize: 15, marginBottom: 14, color: colors.text, backgroundColor: colors.card },
  multiline: { height: 90, textAlignVertical: 'top' },
  submitButton: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 12 },
  submitButtonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
  cancel: { textAlign: 'center', color: colors.textSecondary, fontSize: 14, marginBottom: 8 },
});
