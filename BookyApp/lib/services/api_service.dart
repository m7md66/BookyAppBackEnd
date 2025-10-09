import 'dart:convert';
import 'package:http/http.dart' as http;

class ApiService {
  static final ApiService _instance = ApiService._internal();
  factory ApiService() => _instance;
  ApiService._internal();

  static const String baseUrl = 'http://your-api-url.com/api'; // Replace with your backend URL

  Future<http.Response> get(String endpoint, {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl$endpoint');
    try {
      final response = await http.get(url, headers: headers);
      return response;
    } catch (e) {
      throw Exception('GET $endpoint failed: $e');
    }
  }

  Future<http.Response> post(String endpoint, {Map<String, String>? headers, Object? body}) async {
    final url = Uri.parse('$baseUrl$endpoint');
    try {
      final response = await http.post(url, headers: headers, body: body);
      return response;
    } catch (e) {
      throw Exception('POST $endpoint failed: $e');
    }
  }
} 