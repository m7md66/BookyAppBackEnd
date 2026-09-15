import client from './client';

export const getAllInterests = () => client.get('/Genres/GetAllInterests');
export const saveInterests = (ids) => client.post('/Genres/MakeInterests', ids);
export const getUserInterests = () => client.get('/Genres/GetUserInterests');
export const addInterest = (name) => client.post('/Genres/addInterest', null, { params: { name } });
