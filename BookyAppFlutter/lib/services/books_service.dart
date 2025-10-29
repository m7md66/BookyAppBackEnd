import 'dart:convert';
import '../models/book.dart';
import 'api_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class BooksService {
  final ApiService _api = ApiService();
  final FlutterSecureStorage _storage = const FlutterSecureStorage();

  Future<List<Book>> getAllBooks() async {
    final token = await _storage.read(key: 'token');
    final response = await _api.get('/Books/getAllBooks', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final books = (data['data'] as List).map((e) => Book.fromJson(e)).toList();
      return books;
    } else {
      throw Exception('Failed to load books');
    }
  }

  Future<bool> favorBook(String bookId) async {
    final token = await _storage.read(key: 'token');
    final response = await _api.post('/Books/FavorBook?bookId=$bookId', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      return data['success'] ?? false;
    } else {
      return false;
    }
  }

  Future<List<Book>> getFavoriteBooks() async {
    final token = await _storage.read(key: 'token');
    final response = await _api.get('/Books/GetFavoriteBooks', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final books = (data['data'] as List).map((e) => Book.fromJson(e)).toList();
      return books;
    } else {
      throw Exception('Failed to load favorite books');
    }
  }
} 