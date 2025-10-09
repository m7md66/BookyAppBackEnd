import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/books_provider.dart';

class BooksExploreScreen extends StatefulWidget {
  const BooksExploreScreen({super.key});

  @override
  State<BooksExploreScreen> createState() => _BooksExploreScreenState();
}

class _BooksExploreScreenState extends State<BooksExploreScreen> {
  @override
  void initState() {
    super.initState();
    Provider.of<BooksProvider>(context, listen: false).loadBooks();
  }

  @override
  Widget build(BuildContext context) {
    final provider = Provider.of<BooksProvider>(context);
    return Scaffold(
      appBar: AppBar(
        title: const Text('Explore Books'),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: provider.isLoading
          ? const Center(child: CircularProgressIndicator())
          : ListView.builder(
              itemCount: provider.books.length,
              itemBuilder: (context, index) {
                final book = provider.books[index];
                return ListTile(
                  title: Text(book.title),
                  subtitle: Text('by ${book.author}'),
                  trailing: IconButton(
                    icon: Icon(
                      book.isFavorite ? Icons.bookmark : Icons.bookmark_border,
                      color: Colors.blue,
                    ),
                    onPressed: () => provider.favorBook(book.id),
                  ),
                );
              },
            ),
    );
  }
} 