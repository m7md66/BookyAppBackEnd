import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import i18n from '../i18n';

const BASE_URL = 'http://10.0.2.2:5212/api'; // Android emulator → localhost

// Public origin that serves the shareable quote page (GET /q/{id}).
// Replace with the real domain once the backend is deployed and the app is published.
export const SHARE_BASE_URL = 'https://REPLACE_WITH_PUBLIC_DOMAIN';

const client = axios.create({ baseURL: BASE_URL });

client.interceptors.request.use(async (config) => {
  const token = await AsyncStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  config.headers['Accept-Language'] = i18n.language || 'en';
  return config;
});

export default client;
