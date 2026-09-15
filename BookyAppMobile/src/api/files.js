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

export const uploadAvatar = (file) => {
  const formData = new FormData();
  formData.append('file', {
    uri: file.uri,
    name: file.name,
    type: file.mimeType || 'image/jpeg',
  });
  return client.post('/Files/uploadAvatar', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
};
