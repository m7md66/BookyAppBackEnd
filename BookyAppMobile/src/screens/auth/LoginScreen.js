import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, ActivityIndicator, KeyboardAvoidingView, Platform } from 'react-native';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../../store/authStore';
import { colors, radius } from '../../theme';

export default function LoginScreen({ navigation }) {
  const { t } = useTranslation();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login, isLoading, error } = useAuthStore();

  const handleLogin = async () => {
    await login(email, password);
  };

  return (
    <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <Text style={styles.title}>{t('auth.appName')}</Text>
      <Text style={styles.subtitle}>{t('auth.welcomeBack')}</Text>

      {error ? <Text style={styles.error}>{error}</Text> : null}

      <TextInput
        style={styles.input}
        placeholder={t('auth.email')}
        placeholderTextColor={colors.textSecondary}
        autoCapitalize="none"
        keyboardType="email-address"
        value={email}
        onChangeText={setEmail}
      />
      <TextInput
        style={styles.input}
        placeholder={t('auth.password')}
        placeholderTextColor={colors.textSecondary}
        secureTextEntry
        value={password}
        onChangeText={setPassword}
      />

      <TouchableOpacity style={styles.button} onPress={handleLogin} disabled={isLoading}>
        {isLoading ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('auth.login')}</Text>}
      </TouchableOpacity>

      <TouchableOpacity onPress={() => navigation.navigate('Register')}>
        <Text style={styles.link}>{t('auth.toRegister')}</Text>
      </TouchableOpacity>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background, justifyContent: 'center', paddingHorizontal: 24 },
  title: { fontSize: 32, fontWeight: 'bold', color: colors.text, marginBottom: 4 },
  subtitle: { fontSize: 16, color: colors.textSecondary, marginBottom: 32 },
  input: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, fontSize: 15, marginBottom: 14, color: colors.text, backgroundColor: colors.card },
  button: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 16 },
  buttonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
  error: { color: colors.danger, marginBottom: 12, fontSize: 14 },
  link: { textAlign: 'center', color: colors.primary, fontSize: 14 },
});
