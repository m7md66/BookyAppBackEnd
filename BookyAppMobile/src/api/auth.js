import client from './client';

export const login = (data) => client.post('/Account/Login', data);
export const register = (data) => client.post('/Account/AddUser', data);
export const getMyProfile = () => client.get('/Account/Me');

// Email verification (only exercised when the backend's Features:RequireEmailConfirmation is on).
export const confirmEmail = (data) => client.post('/Account/ConfirmEmail', data); // { email, code }
export const resendOtp = (data) => client.post('/Account/ResendOtp', data); // { email }
