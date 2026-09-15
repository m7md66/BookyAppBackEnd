import React, { useState, useCallback } from 'react';
import { FlatList, View, Text, StyleSheet, ActivityIndicator, RefreshControl, TouchableOpacity, Alert } from 'react-native';
import { useFocusEffect, useNavigation } from '@react-navigation/native';
import { Ionicons } from '@expo/vector-icons';
import { useTranslation } from 'react-i18next';
import * as ImagePicker from 'expo-image-picker';
import { getMyProfile, updateAvatar } from '../../api/auth';
import { uploadAvatar } from '../../api/files';
import { getMyQuotations, getMyLikedQuotations, getMyRequotedQuotations } from '../../api/quotations';
import QuotationCard from '../../components/QuotationCard';
import Avatar from '../../components/Avatar';
import { useAuthStore } from '../../store/authStore';
import { useLanguageStore } from '../../store/languageStore';
import { colors, radius, shadow } from '../../theme';

const PAGE_SIZE = 20;

const TABS = [
  { key: 'mine', labelKey: 'profile.tabs.mine', fetch: getMyQuotations },
  { key: 'liked', labelKey: 'profile.tabs.liked', fetch: getMyLikedQuotations },
  { key: 'requoted', labelKey: 'profile.tabs.requoted', fetch: getMyRequotedQuotations },
];

function ProfileHeader({ profile, onLogout, onAvatarPress, avatarUploading, onEditInterests }) {
  const { t } = useTranslation();
  const { language, setLanguage } = useLanguageStore();
  const fullName = profile ? `${profile.firstName ?? ''} ${profile.lastName ?? ''}`.trim() : '';

  return (
    <View>
      <View style={styles.profileCard}>
        <TouchableOpacity onPress={onAvatarPress} disabled={avatarUploading} style={styles.avatarWrapper}>
          <Avatar imageUrl={profile?.imageUrl} name={fullName} size={52} />
          {avatarUploading ? (
            <View style={styles.avatarOverlay}>
              <ActivityIndicator size="small" color={colors.onPrimary} />
            </View>
          ) : (
            <View style={styles.avatarEditBadge}>
              <Text style={styles.avatarEditIcon}>✎</Text>
            </View>
          )}
        </TouchableOpacity>
        <View style={styles.profileInfo}>
          <Text style={styles.profileName}>{fullName || '...'}</Text>
          <Text style={styles.profileEmail}>{profile?.email ?? ''}</Text>
        </View>
        <TouchableOpacity onPress={onLogout}>
          <Text style={styles.logout}>{t('profile.logout')}</Text>
        </TouchableOpacity>
      </View>

      <TouchableOpacity style={styles.interestsRow} onPress={onEditInterests}>
        <Text style={styles.langLabel}>{t('profile.myInterests')}</Text>
        <Ionicons name="chevron-forward" size={18} color={colors.textSecondary} />
      </TouchableOpacity>

      <View style={styles.langRow}>
        <Text style={styles.langLabel}>{t('profile.language')}</Text>
        <View style={styles.langToggle}>
          <TouchableOpacity
            style={[styles.langOption, language === 'en' && styles.langOptionActive]}
            onPress={() => setLanguage('en')}
          >
            <Text style={[styles.langText, language === 'en' && styles.langTextActive]}>{t('profile.english')}</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.langOption, language === 'ar' && styles.langOptionActive]}
            onPress={() => setLanguage('ar')}
          >
            <Text style={[styles.langText, language === 'ar' && styles.langTextActive]}>{t('profile.arabic')}</Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
}

export default function ProfileScreen() {
  const { t } = useTranslation();
  const navigation = useNavigation();
  const logout = useAuthStore((state) => state.logout);
  const updateStoredUser = useAuthStore((state) => state.updateUser);
  const [profile, setProfile] = useState(null);
  const [activeTab, setActiveTab] = useState('mine');
  const [quotations, setQuotations] = useState([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const [avatarUploading, setAvatarUploading] = useState(false);

  const handleAvatarPress = async () => {
    const permission = await ImagePicker.requestMediaLibraryPermissionsAsync();
    if (!permission.granted) {
      Alert.alert(t('profile.avatarPermissionTitle'), t('profile.avatarPermissionMessage'));
      return;
    }

    const result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ['images'],
      allowsEditing: true,
      aspect: [1, 1],
      quality: 0.7,
    });
    if (result.canceled) return;

    const asset = result.assets[0];
    setAvatarUploading(true);
    try {
      const uploadRes = await uploadAvatar({
        uri: asset.uri,
        name: asset.fileName ?? `avatar-${Date.now()}.jpg`,
        mimeType: asset.mimeType,
      });
      const fileUrl = uploadRes.data?.fileUrl;
      const updateRes = await updateAvatar(fileUrl);
      const updated = updateRes.data?.data ?? updateRes.data;
      setProfile(updated);
      updateStoredUser({ imageUrl: updated.imageUrl });
    } catch {
      Alert.alert(t('profile.avatarUploadFailed'));
    } finally {
      setAvatarUploading(false);
    }
  };

  const load = useCallback(async (tabKey, pageNum = 1, replace = false) => {
    const tab = TABS.find((item) => item.key === tabKey);
    try {
      const res = await tab.fetch(pageNum, PAGE_SIZE);
      const items = res.data.dataResult?.items ?? res.data.data ?? [];
      setQuotations((prev) => {
        if (replace) return items;
        const seen = new Set(prev.map((q) => q.id));
        return [...prev, ...items.filter((q) => !seen.has(q.id))];
      });
      setHasMore(items.length === PAGE_SIZE);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      getMyProfile().then((res) => setProfile(res.data.data ?? res.data));
    }, [])
  );

  useFocusEffect(
    useCallback(() => {
      setLoading(true);
      setPage(1);
      load(activeTab, 1, true);
    }, [activeTab, load])
  );

  const onRefresh = () => {
    setRefreshing(true);
    setPage(1);
    load(activeTab, 1, true);
  };

  const onEndReached = () => {
    if (!hasMore || loading) return;
    const next = page + 1;
    setPage(next);
    load(activeTab, next);
  };

  return (
    <FlatList
      data={quotations}
      keyExtractor={(item) => item.id ?? Math.random().toString()}
      renderItem={({ item }) => <QuotationCard quotation={item} />}
      contentContainerStyle={styles.list}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
      onEndReached={onEndReached}
      onEndReachedThreshold={0.5}
      ListHeaderComponent={
        <View>
          <Text style={styles.header}>{t('profile.title')}</Text>
          <ProfileHeader
            profile={profile}
            onLogout={logout}
            onAvatarPress={handleAvatarPress}
            avatarUploading={avatarUploading}
            onEditInterests={() => navigation.navigate('EditInterests')}
          />
          <View style={styles.tabs}>
            {TABS.map((tab) => (
              <TouchableOpacity
                key={tab.key}
                style={[styles.tab, activeTab === tab.key && styles.tabActive]}
                onPress={() => setActiveTab(tab.key)}
              >
                <Text style={[styles.tabText, activeTab === tab.key && styles.tabTextActive]}>{t(tab.labelKey)}</Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>
      }
      ListEmptyComponent={
        loading ? (
          <ActivityIndicator size="large" color={colors.primary} style={styles.loadingIndicator} />
        ) : (
          <Text style={styles.empty}>{t('profile.empty')}</Text>
        )
      }
    />
  );
}

const styles = StyleSheet.create({
  list: { paddingHorizontal: 16, paddingTop: 56, paddingBottom: 24 },
  header: { fontSize: 26, fontWeight: 'bold', color: colors.text, marginBottom: 16 },
  profileCard: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: colors.card,
    borderRadius: radius.lg,
    padding: 16,
    marginBottom: 16,
    ...shadow.card,
  },
  avatarWrapper: { position: 'relative' },
  avatarOverlay: {
    ...StyleSheet.absoluteFillObject,
    borderRadius: 26,
    backgroundColor: colors.overlay,
    justifyContent: 'center',
    alignItems: 'center',
  },
  avatarEditBadge: {
    position: 'absolute',
    bottom: -2,
    right: -2,
    width: 20,
    height: 20,
    borderRadius: 10,
    backgroundColor: colors.primary,
    justifyContent: 'center',
    alignItems: 'center',
    borderWidth: 2,
    borderColor: colors.card,
  },
  avatarEditIcon: { color: colors.onPrimary, fontSize: 10 },
  profileInfo: { flex: 1, marginStart: 12 },
  profileName: { fontSize: 16, fontWeight: '600', color: colors.text },
  profileEmail: { fontSize: 13, color: colors.textSecondary, marginTop: 2 },
  logout: { fontSize: 14, fontWeight: '600', color: colors.primary },
  interestsRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    backgroundColor: colors.card,
    borderRadius: radius.md,
    padding: 14,
    marginBottom: 12,
    ...shadow.card,
  },
  langRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginBottom: 16 },
  langLabel: { fontSize: 14, fontWeight: '600', color: colors.text },
  langToggle: { flexDirection: 'row', backgroundColor: colors.track, borderRadius: radius.md, padding: 4 },
  langOption: { paddingVertical: 6, paddingHorizontal: 16, borderRadius: radius.sm },
  langOptionActive: { backgroundColor: colors.card, shadowColor: '#000', shadowOpacity: 0.08, shadowRadius: 4, elevation: 2 },
  langText: { fontSize: 13, fontWeight: '600', color: colors.textSecondary },
  langTextActive: { color: colors.primary },
  tabs: { flexDirection: 'row', backgroundColor: colors.track, borderRadius: radius.md, padding: 4, marginBottom: 16 },
  tab: { flex: 1, paddingVertical: 10, borderRadius: radius.sm, alignItems: 'center' },
  tabActive: { backgroundColor: colors.card, shadowColor: '#000', shadowOpacity: 0.08, shadowRadius: 4, elevation: 2 },
  tabText: { fontSize: 13, fontWeight: '600', color: colors.textSecondary },
  tabTextActive: { color: colors.primary },
  loadingIndicator: { marginTop: 40 },
  empty: { textAlign: 'center', color: colors.textSecondary, fontSize: 14, marginTop: 40 },
});
