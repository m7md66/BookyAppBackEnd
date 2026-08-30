import client from './client';

export const createQuotation = (bookId, content) =>
  client.post('/Quotation/CreateQuotation', { BookId: bookId, Content: content });

export const getFeed = (pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetFeed', { params: { 'Pagenation.pageNumber': pageNumber, 'Pagenation.pageSize': pageSize } });

export const likeQuotation = (id) => client.post(`/Quotation/LikeQuotation?QuotationId=${id}`);
export const requoteQuotation = (id, comment) =>
  client.post('/Quotation/RequoteQuotation', { QuotationId: id, Comment: comment || null });
export const shareQuotation = (id) => client.post(`/Quotation/ShareQuotation?QuotationId=${id}`);
export const commentQuotation = (data) => client.post('/Quotation/CommentQuotation', data);
export const getComments = (quotationId, pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetComments', { params: { quotationId, 'pagenation.pageNumber': pageNumber, 'pagenation.pageSize': pageSize } });

export const getMyQuotations = (pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetMyQuotation', { params: { 'pagenation.pageNumber': pageNumber, 'pagenation.pageSize': pageSize } });

export const getMyLikedQuotations = (pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetMyLikedQuotations', { params: { 'pagenation.pageNumber': pageNumber, 'pagenation.pageSize': pageSize } });

export const getMyRequotedQuotations = (pageNumber = 1, pageSize = 20) =>
  client.get('/Quotation/GetMyRequotedQuotations', { params: { 'pagenation.pageNumber': pageNumber, 'pagenation.pageSize': pageSize } });
