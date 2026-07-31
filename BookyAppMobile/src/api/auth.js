import client from './client';

export const login = (data) => client.post('/Account/Login', data);
export const register = (data) => client.post('/Account/AddUser', data);
