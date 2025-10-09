import 'dart:convert';
import '../models/genre.dart';
import '../models/api_response.dart';
import 'api_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class GenresService {
  final ApiService _api = ApiService();
  final FlutterSecureStorage _storage = const FlutterSecureStorage();

  Future<List<Genre>> getAllGenres() async {
    final token = await _storage.read(key: 'token');
    final response = await _api.get('/Genres/GetAllInterests', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final genres = (data['data'] as List).map((e) => Genre.fromJson(e)).toList();
      return genres;
    } else {
      throw Exception('Failed to load genres');
    }
  }

  Future<bool> makeInterest(String genreId) async {
    final token = await _storage.read(key: 'token');
    final response = await _api.post('/Genres/MakeInterest?GenreId=$genreId', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      return data['success'] ?? false;
    } else {
      return false;
    }
  }

  Future<bool> makeInterests(List<String> genreIds) async {
    final token = await _storage.read(key: 'token');
    final response = await _api.post('/Genres/MakeInterests',
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: jsonEncode(genreIds));
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      return data['success'] ?? false;
    } else {
      return false;
    }
  }

  Future<List<Genre>> getUserInterests() async {
    final token = await _storage.read(key: 'token');
    final response = await _api.get('/Genres/GetUserInterests', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final genres = (data['data'] as List).map((e) => Genre.fromJson(e)).toList();
      return genres;
    } else {
      throw Exception('Failed to load user interests');
    }
  }
} 