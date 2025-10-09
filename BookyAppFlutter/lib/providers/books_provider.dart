import 'package:flutter/material.dart';
import '../models/book.dart';
import '../services/books_service.dart';

class BooksProvider extends ChangeNotifier {
  final BooksService _booksService = BooksService();
  List<Book> _books = [];
  bool _isLoading = false;
  String? _error;

  List<Book> get books => _books;
  bool get isLoading => _isLoading;
  String? get error => _error;

  Future<void> loadBooks() async {
    _isLoading = true;
    _error = null;
    notifyListeners();
    try {
      _books = await _booksService.getAllBooks();
    } catch (e) {
      _error = e.toString();
    }
    _isLoading = false;
    notifyListeners();
  }

  Future<void> favorBook(String bookId) async {
    final result = await _booksService.favorBook(bookId);
    if (result) {
      await loadBooks();
    }
  }
} 