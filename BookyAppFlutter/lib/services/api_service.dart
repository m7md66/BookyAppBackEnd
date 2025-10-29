import 'package:http/http.dart' as http;

class ApiService {
  static final ApiService _instance = ApiService._internal();
  factory ApiService() => _instance;
  ApiService._internal();

  static const String baseUrl =
      // 'http://127.0.0.1:5001/api'; // Replace with your backend URL
      'http://10.0.2.2:5212/api'; // Replace with your backend URL

  Future<http.Response> get(String endpoint,
      {Map<String, String>? headers}) async {
    final url = Uri.parse('$baseUrl$endpoint');
    try {
      final response = await http.get(url, headers: headers);
      return response;
    } catch (e) {
      throw Exception('GET $endpoint failed: $e');
    }
  }

  Future<http.Response> post(String endpoint,
      {Map<String, String>? headers, Object? body}) async {
    final url = Uri.parse('$baseUrl$endpoint');
    try {
      final response = await http.post(url, headers: headers, body: body);
      return response;
    } catch (e) {
      throw Exception('POST $endpoint failed: $e');
    }
  }
}
