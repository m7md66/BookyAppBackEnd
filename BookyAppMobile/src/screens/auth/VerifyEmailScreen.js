import React, { useEffect, useState } from 'react';
import {
  View, Text, TextInput, TouchableOpacity, StyleSheet, ActivityIndicator,
  KeyboardAvoidingView, Platform,
} from 'react-native';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../../store/authStore';
import { colors, radius } from '../../theme';

const RESEND_COOLDOWN = 60;

export default function VerifyEmailScreen() {
  const { t } = useTranslation();
  const { pendingEmail, verifyEmail, resendCode, cancelEmailVerification, isLoading, error } = useAuthStore();

  const [code, setCode] = useState('');
  const [notice, setNotice] = useState('');
  const [secondsLeft, setSecondsLeft] = useState(RESEND_COOLDOWN);

  useEffect(() => {
    if (secondsLeft <= 0) return;
    const id = setTimeout(() => setSecondsLeft(secondsLeft - 1), 1000);
    return () => clearTimeout(id);
  }, [secondsLeft]);

  const startCooldown = () => setSecondsLeft(RESEND_COOLDOWN);

  const handleVerify = () => {
    setNotice('');
    verifyEmail(code.trim());
  };

  const handleResend = async () => {
    setNotice('');
    const res = await resendCode();
    setNotice(res.message);
    if (res.ok) startCooldown();
  };

  return (
    <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <Text style={styles.title}>{t('auth.verifyEmail.title')}</Text>
      <Text style={styles.subtitle}>{t('auth.verifyEmail.subtitle', { email: pendingEmail })}</Text>

      {error ? <Text style={styles.error}>{error}</Text> : null}
      {notice ? <Text style={styles.notice}>{notice}</Text> : null}

      <TextInput
        style={styles.input}
        placeholder={t('auth.verifyEmail.codePlaceholder')}
        placeholderTextColor={colors.textSecondary}
        keyboardType="number-pad"
        maxLength={6}
        value={code}
        onChangeText={(v) => setCode(v.replace(/[^0-9]/g, ''))}
      />

      <TouchableOpacity
        style={[styles.button, (code.length !== 6 || isLoading) && styles.buttonDisabled]}
        onPress={handleVerify}
        disabled={code.length !== 6 || isLoading}
      >
        {isLoading ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('auth.verifyEmail.verify')}</Text>}
      </TouchableOpacity>

      <TouchableOpacity onPress={handleResend} disabled={secondsLeft > 0}>
        <Text style={[styles.link, secondsLeft > 0 && styles.linkMuted]}>
          {secondsLeft > 0 ? t('auth.verifyEmail.resendIn', { seconds: secondsLeft }) : t('auth.verifyEmail.resend')}
        </Text>
      </TouchableOpacity>

      <TouchableOpacity onPress={cancelEmailVerification}>
        <Text style={styles.link}>{t('auth.verifyEmail.changeEmail')}</Text>
      </TouchableOpacity>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background, justifyContent: 'center', paddingHorizontal: 24 },
  title: { fontSize: 32, fontWeight: 'bold', color: colors.text, marginBottom: 4 },
  subtitle: { fontSize: 16, color: colors.textSecondary, marginBottom: 32 },
  input: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, fontSize: 20, letterSpacing: 6, textAlign: 'center', marginBottom: 14, color: colors.text, backgroundColor: colors.card },
  button: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 16 },
  buttonDisabled: { backgroundColor: colors.primaryDisabled },
  buttonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
  error: { color: colors.danger, marginBottom: 12, fontSize: 14 },
  notice: { color: colors.primary, marginBottom: 12, fontSize: 14 },
  link: { textAlign: 'center', color: colors.primary, fontSize: 14, marginTop: 8 },
  linkMuted: { color: colors.textSecondary },
});
