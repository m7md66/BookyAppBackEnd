import React, { useState } from 'react';
import { View, Text, Pressable, StyleSheet } from 'react-native';
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

export default function QuotationCard({ quotation }) {
  const navigation = useNavigation();
  const [likes, setLikes] = useState(quotation.likesNumber ?? 0);
  const [requotes, setRequotes] = useState(quotation.reQueteNumber ?? 0);
  const [shares, setShares] = useState(quotation.sharesNumber ?? 0);
  const [liked, setLiked] = useState(false);
  const [requoted, setRequoted] = useState(false);

  const handleLike = async () => {
    setLiked((v) => !v);
    setLikes((v) => liked ? v - 1 : v + 1);
    await likeQuotation(quotation.id);
  };

  const handleRequote = async () => {
    setRequoted((v) => !v);
    setRequotes((v) => requoted ? v - 1 : v + 1);
    await requoteQuotation(quotation.id);
  };

  const handleShare = async () => {
    setShares((v) => v + 1);
    await shareQuotation(quotation.id);
  };

  const handleBookPress = () => {
    navigation.navigate('Browse', { highlightBookId: quotation.bookId });
  };

  return (
    <View style={styles.card}>
      {quotation.userFullName ? <Text style={styles.userName}>{quotation.userFullName}</Text> : null}
      <Text style={styles.content}>"{quotation.content}"</Text>
      <Pressable style={styles.bookRow} onPress={handleBookPress}>
        <Text style={styles.bookTitle}>{quotation.bookTitle}</Text>
        {quotation.bookAuther ? <Text style={styles.author}> · {quotation.bookAuther}</Text> : null}
      </Pressable>
      <View style={styles.actions}>
        <ActionButton label="Like" onPress={handleLike}>
          <Text style={[styles.actionIcon, liked && styles.active]}>♥</Text>
          <Text style={styles.actionCount}>{likes}</Text>
        </ActionButton>
        <ActionButton label="Re-quote" onPress={handleRequote}>
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
});
