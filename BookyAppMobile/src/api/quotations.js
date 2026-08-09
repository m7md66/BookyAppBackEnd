import client from './client';

export const createQuotation = (bookId, content) =>
  client.post('/Quotation/CreateQuotation', { BookId: bookId, Content: content });

export const getFeed = (pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetFeed', { params: { 'Pagenation.pageNumber': pageNumber, 'Pagenation.pageSize': pageSize } });

export const likeQuotation = (id) => client.post(`/Quotation/LikeQuotation?QuotationId=${id}`);
export const requoteQuotation = (id) => client.post(`/Quotation/RequoteQuotation?QuotationId=${id}`);
export const shareQuotation = (id) => client.post(`/Quotation/ShareQuotation?QuotationId=${id}`);
export const commentQuotation = (data) => client.post('/Quotation/CommentQuotation', data);
export const getComments = (quotationId, pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetComments', { params: { quotationId, 'pagenation.pageNumber': pageNumber, 'pagenation.pageSize': pageSize } });
