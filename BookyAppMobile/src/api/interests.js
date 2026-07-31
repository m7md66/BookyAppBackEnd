import client from './client';

export const getAllInterests = () => client.get('/Genres/GetAllInterests');
export const saveInterests = (ids) => client.post('/Genres/MakeInterests', ids);
