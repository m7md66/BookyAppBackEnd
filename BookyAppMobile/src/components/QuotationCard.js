import React, { useState } from 'react';
import { View, Text, Pressable, StyleSheet, Modal, TextInput, KeyboardAvoidingView, Platform, TouchableOpacity, ActivityIndicator } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { likeQuotation, requoteQuotation, shareQuotation } from '../api/quotations';

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
          <Text style={styles.modalTitle}>Re-quote</Text>
          <View style={styles.previewCard}>
            <QuotePreview quotation={quotation} />
          </View>
          <TextInput
            style={[styles.input, styles.multiline]}
            placeholder="Add a comment (optional)"
            placeholderTextColor="#888"
            value={comment}
            onChangeText={setComment}
            multiline
          />
          <TouchableOpacity style={styles.submitButton} onPress={handleSubmit} disabled={submitting}>
            {submitting ? <ActivityIndicator color="#fff" /> : <Text style={styles.submitButtonText}>Re-quote</Text>}
          </TouchableOpacity>
          <TouchableOpacity onPress={onClose} disabled={submitting}>
            <Text style={styles.cancel}>Cancel</Text>
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}

export default function QuotationCard({ quotation }) {
  const navigation = useNavigation();
  const [likes, setLikes] = useState(quotation.likesNumber ?? 0);
  const [requotes, setRequotes] = useState(quotation.reQueteNumber ?? 0);
  const [shares, setShares] = useState(quotation.sharesNumber ?? 0);
  const [liked, setLiked] = useState(false);
  const [requoted, setRequoted] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);

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
    setShares((v) => v + 1);
    await shareQuotation(targetQuotationId);
  };

  const handleBookPress = () => {
    navigation.navigate('Browse', { highlightBookId: quotation.bookId });
  };

  return (
    <View style={styles.card}>
      {quotation.isRequote ? (
        <View style={styles.requoteHeader}>
          <Text style={styles.requoteHeaderIcon}>↺</Text>
          <Text style={styles.requoteHeaderText}>{quotation.requoterFullName} re-quoted</Text>
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
        <ActionButton label="Like" onPress={handleLike}>
          <Text style={[styles.actionIcon, liked && styles.active]}>♥</Text>
          <Text style={styles.actionCount}>{likes}</Text>
        </ActionButton>
        <ActionButton label="Re-quote" onPress={() => setModalVisible(true)}>
          <Text style={[styles.actionIcon, requoted && styles.active]}>↺</Text>
          <Text style={styles.actionCount}>{requotes}</Text>
        </ActionButton>
        <ActionButton label="Share" onPress={handleShare}>
          <Text style={styles.actionIcon}>↗</Text>
          <Text style={styles.actionCount}>{shares}</Text>
        </ActionButton>
        <ActionButton label="Comment">
          <Text style={styles.actionIcon}>💬</Text>
          <Text style={styles.actionCount}>{quotation.commentsNumber ?? 0}</Text>
        </ActionButton>
      </View>

      <RequoteModal
        visible={modalVisible}
        quotation={quotation}
        onClose={() => setModalVisible(false)}
        onSubmit={handleRequoteSubmit}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  card: { backgroundColor: '#fff', borderRadius: 14, padding: 18, marginBottom: 14, shadowColor: '#000', shadowOpacity: 0.06, shadowRadius: 8, shadowOffset: { width: 0, height: 2 }, elevation: 3 },
  userName: { fontSize: 13, fontWeight: '600', color: '#1a1a1a', marginBottom: 6 },
  content: { fontSize: 16, color: '#1a1a1a', lineHeight: 24, marginBottom: 12, fontStyle: 'italic' },
  bookRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 14 },
  bookTitle: { fontSize: 13, fontWeight: '600', color: '#4F46E5' },
  author: { fontSize: 13, color: '#888' },
  actions: { flexDirection: 'row', gap: 20 },
  actionWrapper: { alignItems: 'center', position: 'relative' },
  action: { flexDirection: 'row', alignItems: 'center', gap: 4 },
  actionIcon: { fontSize: 18, color: '#888' },
  actionCount: { fontSize: 13, color: '#888' },
  active: { color: '#ef4444' },
  requoteHeader: { flexDirection: 'row', alignItems: 'center', gap: 6, marginBottom: 8 },
  requoteHeaderIcon: { fontSize: 13, color: '#888' },
  requoteHeaderText: { fontSize: 13, fontWeight: '600', color: '#888' },
  requoteComment: { fontSize: 15, color: '#1a1a1a', lineHeight: 22, marginBottom: 12 },
  nestedCard: { borderWidth: 1, borderColor: '#ddd', borderRadius: 10, padding: 14 },
  tooltip: {
    position: 'absolute',
    bottom: '100%',
    marginBottom: 10,
    backgroundColor: '#fff',
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
  tooltipText: { fontSize: 12, color: '#1a1a1a', fontWeight: '500' },
  tooltipArrow: {
    position: 'absolute',
    bottom: -4,
    width: 8,
    height: 8,
    backgroundColor: '#fff',
    transform: [{ rotate: '45deg' }],
  },
  modalOverlay: { flex: 1, justifyContent: 'flex-end', backgroundColor: 'rgba(0,0,0,0.4)' },
  modalCard: { backgroundColor: '#fff', borderTopLeftRadius: 20, borderTopRightRadius: 20, padding: 24 },
  modalTitle: { fontSize: 20, fontWeight: 'bold', color: '#1a1a1a', marginBottom: 16 },
  previewCard: { borderWidth: 1, borderColor: '#ddd', borderRadius: 10, padding: 14, marginBottom: 14 },
  input: { borderWidth: 1, borderColor: '#ddd', borderRadius: 10, padding: 14, fontSize: 15, marginBottom: 14, color: '#1a1a1a' },
  multiline: { height: 90, textAlignVertical: 'top' },
  submitButton: { backgroundColor: '#4F46E5', borderRadius: 10, padding: 16, alignItems: 'center', marginBottom: 12 },
  submitButtonText: { color: '#fff', fontWeight: '600', fontSize: 16 },
  cancel: { textAlign: 'center', color: '#888', fontSize: 14, marginBottom: 8 },
});
