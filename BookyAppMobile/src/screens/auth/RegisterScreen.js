import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, ActivityIndicator, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../../store/authStore';
import { colors, radius } from '../../theme';

export default function RegisterScreen({ navigation }) {
  const { t } = useTranslation();
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { register, isLoading, error } = useAuthStore();

  const handleRegister = async () => {
    await register(firstName, lastName, email, password);
    // RootNavigator switches to InterestsScreen once needsInterests is set
  };

  return (
    <KeyboardAvoidingView style={{ flex: 1 }} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <ScrollView contentContainerStyle={styles.container} keyboardShouldPersistTaps="handled">
        <Text style={styles.title}>{t('auth.createAccount')}</Text>
        <Text style={styles.subtitle}>{t('auth.joinBooky')}</Text>

        {error ? <Text style={styles.error}>{error}</Text> : null}

        <TextInput style={styles.input} placeholder={t('auth.firstName')} placeholderTextColor={colors.textSecondary} value={firstName} onChangeText={setFirstName} />
        <TextInput style={styles.input} placeholder={t('auth.lastName')} placeholderTextColor={colors.textSecondary} value={lastName} onChangeText={setLastName} />
        <TextInput style={styles.input} placeholder={t('auth.email')} placeholderTextColor={colors.textSecondary} autoCapitalize="none" keyboardType="email-address" value={email} onChangeText={setEmail} />
        <TextInput style={styles.input} placeholder={t('auth.password')} placeholderTextColor={colors.textSecondary} secureTextEntry value={password} onChangeText={setPassword} />

        <TouchableOpacity style={styles.button} onPress={handleRegister} disabled={isLoading}>
          {isLoading ? <ActivityIndicator color={colors.onPrimary} /> : <Text style={styles.buttonText}>{t('auth.register')}</Text>}
        </TouchableOpacity>

        <TouchableOpacity onPress={() => navigation.navigate('Login')}>
          <Text style={styles.link}>{t('auth.toLogin')}</Text>
        </TouchableOpacity>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flexGrow: 1, backgroundColor: colors.background, justifyContent: 'center', paddingHorizontal: 24, paddingVertical: 40 },
  title: { fontSize: 32, fontWeight: 'bold', color: colors.text, marginBottom: 4 },
  subtitle: { fontSize: 16, color: colors.textSecondary, marginBottom: 32 },
  input: { borderWidth: 1, borderColor: colors.border, borderRadius: radius.md, padding: 14, fontSize: 15, marginBottom: 14, color: colors.text, backgroundColor: colors.card },
  button: { backgroundColor: colors.primary, borderRadius: radius.md, padding: 16, alignItems: 'center', marginBottom: 16 },
  buttonText: { color: colors.onPrimary, fontWeight: '600', fontSize: 16 },
  error: { color: colors.danger, marginBottom: 12, fontSize: 14 },
  link: { textAlign: 'center', color: colors.primary, fontSize: 14 },
});
