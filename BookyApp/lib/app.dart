import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
// Import providers and screens (to be created)

class BookyApp extends StatelessWidget {
  const BookyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        // Add providers here
      ],
      child: MaterialApp(
        title: 'BookyApp',
        theme: ThemeData(
          primarySwatch: Colors.blue,
        ),
        initialRoute: '/splash',
        routes: {
          '/splash': (context) => const PlaceholderWidget('Splash Screen'),
          '/login': (context) => const PlaceholderWidget('Login Screen'),
          '/interests': (context) => const PlaceholderWidget('Interests Screen'),
          '/home': (context) => const PlaceholderWidget('Home Screen'),
          '/books': (context) => const PlaceholderWidget('Books Explore Screen'),
        },
      ),
    );
  }
}

class PlaceholderWidget extends StatelessWidget {
  final String title;
  const PlaceholderWidget(this.title, {super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(title)),
      body: Center(child: Text(title)),
    );
  }
} 