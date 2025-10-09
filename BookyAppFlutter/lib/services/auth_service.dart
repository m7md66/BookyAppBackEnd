import 'dart:convert';
import '../models/user.dart';
import 'api_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthService {
  final ApiService _api = ApiService();
  final FlutterSecureStorage _storage = const FlutterSecureStorage();

  Future<User?> login(String email, String password) async {
    final response = await _api.post('/Account/Login',
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'email': email, 'password': password}));
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final user = User.fromJson(data['data']);
      if (user.token != null) {
        await _storage.write(key: 'token', value: user.token);
      }
      return user;
    } else {
      return null;
    }
  }

  Future<void> logout() async {
    await _storage.delete(key: 'token');
  }

  Future<String?> getToken() async {
    return await _storage.read(key: 'token');
  }
} 