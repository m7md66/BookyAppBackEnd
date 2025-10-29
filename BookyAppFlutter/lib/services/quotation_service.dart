import 'dart:convert';
import '../models/quotation.dart';
import 'api_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class QuotationService {
  final ApiService _api = ApiService();
  final FlutterSecureStorage _storage = const FlutterSecureStorage();

  Future<List<Quotation>> getMyQuotations() async {
    final token = await _storage.read(key: 'token');
    final response = await _api.get('/Quotation/GetMyQuotation', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final quotations = (data['data'] as List).map((e) => Quotation.fromJson(e)).toList();
      return quotations;
    } else {
      throw Exception('Failed to load quotations');
    }
  }

  Future<bool> likeQuotation(String quotationId) async {
    final token = await _storage.read(key: 'token');
    final response = await _api.post('/Quotation/LikeQuotation?QuotationId=$quotationId', headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      return data['success'] ?? false;
    } else {
      return false;
    }
  }

  // Add similar methods for comment, share, requote, etc.
} 