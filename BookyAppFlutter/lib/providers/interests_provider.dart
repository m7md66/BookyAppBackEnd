import 'package:flutter/material.dart';
import '../models/genre.dart';
import '../services/genres_service.dart';

class InterestsProvider extends ChangeNotifier {
  final GenresService _genresService = GenresService();
  List<Genre> _allGenres = [];
  final List<String> _selectedGenreIds = [];
  bool _isLoading = false;
  String? _error;

  List<Genre> get allGenres => _allGenres;
  List<String> get selectedGenreIds => _selectedGenreIds;
  bool get isLoading => _isLoading;
  String? get error => _error;

  Future<void> loadGenres() async {
    _isLoading = true;
    _error = null;
    notifyListeners();
    try {
      _allGenres = await _genresService.getAllGenres();
    } catch (e) {
      _error = e.toString();
    }
    _isLoading = false;
    notifyListeners();
  }

  void toggleGenre(String genreId) {
    if (_selectedGenreIds.contains(genreId)) {
      _selectedGenreIds.remove(genreId);
    } else {
      _selectedGenreIds.add(genreId);
    }
    notifyListeners();
  }

  Future<bool> saveInterests() async {
    _isLoading = true;
    notifyListeners();
    try {
      final result = await _genresService.makeInterests(_selectedGenreIds);
      _isLoading = false;
      notifyListeners();
      return result;
    } catch (e) {
      _isLoading = false;
      _error = e.toString();
      notifyListeners();
      return false;
    }
  }
} 