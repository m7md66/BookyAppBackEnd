import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/interests_provider.dart';

class InterestsScreen extends StatefulWidget {
  const InterestsScreen({super.key});

  @override
  State<InterestsScreen> createState() => _InterestsScreenState();
}

class _InterestsScreenState extends State<InterestsScreen> {
  @override
  void initState() {
    super.initState();
    Provider.of<InterestsProvider>(context, listen: false).loadGenres();
  }

  @override
  Widget build(BuildContext context) {
    final provider = Provider.of<InterestsProvider>(context);
    return Scaffold(
      appBar: AppBar(title: const Text('Select Interests')),
      body: provider.isLoading
          ? const Center(child: CircularProgressIndicator())
          : Column(
              children: [
                if (provider.error != null)
                  Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Text(provider.error!, style: const TextStyle(color: Colors.red)),
                  ),
                Expanded(
                  child: ListView(
                    children: provider.allGenres
                        .map((genre) => CheckboxListTile(
                              title: Text(genre.name),
                              value: provider.selectedGenreIds.contains(genre.id),
                              onChanged: (_) => provider.toggleGenre(genre.id),
                            ))
                        .toList(),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: ElevatedButton(
                    onPressed: provider.selectedGenreIds.isEmpty || provider.isLoading
                        ? null
                        : () async {
                            final success = await provider.saveInterests();
                            if (success) {
                              Navigator.pushReplacementNamed(context, '/home');
                            }
                          },
                    child: const Text('Save Interests'),
                  ),
                ),
              ],
            ),
    );
  }
} 