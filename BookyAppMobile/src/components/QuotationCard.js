import React, { useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { likeQuotation, requoteQuotation, shareQuotation } from '../api/quotations';

export default function QuotationCard({ quotation }) {
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

  return (
    <View style={styles.card}>
      <Text style={styles.content}>"{quotation.content}"</Text>
      <View style={styles.bookRow}>
        <Text style={styles.bookTitle}>{quotation.bookTitle}</Text>
        {quotation.bookAuther ? <Text style={styles.author}> · {quotation.bookAuther}</Text> : null}
      </View>
      <View style={styles.actions}>
        <TouchableOpacity style={styles.action} onPress={handleLike}>
          <Text style={[styles.actionIcon, liked && styles.active]}>♥</Text>
          <Text style={styles.actionCount}>{likes}</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.action} onPress={handleRequote}>
          <Text style={[styles.actionIcon, requoted && styles.active]}>↺</Text>
          <Text style={styles.actionCount}>{requotes}</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.action} onPress={handleShare}>
          <Text style={styles.actionIcon}>↗</Text>
          <Text style={styles.actionCount}>{shares}</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.action}>
          <Text style={styles.actionIcon}>💬</Text>
          <Text style={styles.actionCount}>{quotation.commentsNumber ?? 0}</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  card: { backgroundColor: '#fff', borderRadius: 14, padding: 18, marginBottom: 14, shadowColor: '#000', shadowOpacity: 0.06, shadowRadius: 8, shadowOffset: { width: 0, height: 2 }, elevation: 3 },
  content: { fontSize: 16, color: '#1a1a1a', lineHeight: 24, marginBottom: 12, fontStyle: 'italic' },
  bookRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 14 },
  bookTitle: { fontSize: 13, fontWeight: '600', color: '#4F46E5' },
  author: { fontSize: 13, color: '#888' },
  actions: { flexDirection: 'row', gap: 20 },
  action: { flexDirection: 'row', alignItems: 'center', gap: 4 },
  actionIcon: { fontSize: 18, color: '#888' },
  actionCount: { fontSize: 13, color: '#888' },
  active: { color: '#ef4444' },
});
