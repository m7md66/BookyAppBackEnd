import client from './client';

export const getFavoriteBooks = () => client.get('/Books/GetFavoriteBooks');
export const getBrowseBooks = () => client.get('/Books/GetBrowseBooks');
export const addBook = (book) => client.post('/Books/AddBook', book);
export const favorBook = (bookId) => client.post(`/Books/FavorBook?bookId=${bookId}`);
