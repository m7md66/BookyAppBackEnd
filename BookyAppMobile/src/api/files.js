import client from './client';

export const uploadFile = (file) => {
  const formData = new FormData();
  formData.append('file', {
    uri: file.uri,
    name: file.name,
    type: file.mimeType || 'application/octet-stream',
  });
  return client.post('/Files/upload', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
};
