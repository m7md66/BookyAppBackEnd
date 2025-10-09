import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/quotations_provider.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  void initState() {
    super.initState();
    Provider.of<QuotationsProvider>(context, listen: false).loadQuotations();
  }

  @override
  Widget build(BuildContext context) {
    final provider = Provider.of<QuotationsProvider>(context);
    return Scaffold(
      appBar: AppBar(
        title: const Text('Home'),
        actions: [
          IconButton(
            icon: const Icon(Icons.book),
            onPressed: () => Navigator.pushNamed(context, '/books'),
          ),
        ],
      ),
      body: provider.isLoading
          ? const Center(child: CircularProgressIndicator())
          : ListView.builder(
              itemCount: provider.quotations.length,
              itemBuilder: (context, index) {
                final q = provider.quotations[index];
                return ListTile(
                  title: Text(q.text),
                  subtitle: Text('by ${q.author}'),
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: Icon(q.isLiked ? Icons.favorite : Icons.favorite_border, color: Colors.red),
                        onPressed: () => provider.likeQuotation(q.id),
                      ),
                      Text('${q.likes}'),
                    ],
                  ),
                );
              },
            ),
    );
  }
} 