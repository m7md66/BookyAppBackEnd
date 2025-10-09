import 'package:flutter/material.dart';
import '../models/quotation.dart';
import '../services/quotation_service.dart';

class QuotationsProvider extends ChangeNotifier {
  final QuotationService _quotationService = QuotationService();
  List<Quotation> _quotations = [];
  bool _isLoading = false;
  String? _error;

  List<Quotation> get quotations => _quotations;
  bool get isLoading => _isLoading;
  String? get error => _error;

  Future<void> loadQuotations() async {
    _isLoading = true;
    _error = null;
    notifyListeners();
    try {
      _quotations = await _quotationService.getMyQuotations();
    } catch (e) {
      _error = e.toString();
    }
    _isLoading = false;
    notifyListeners();
  }

  Future<void> likeQuotation(String quotationId) async {
    final result = await _quotationService.likeQuotation(quotationId);
    if (result) {
      await loadQuotations();
    }
  }
} 