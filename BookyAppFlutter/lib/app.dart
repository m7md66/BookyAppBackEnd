import 'package:booky_app/providers/auth_provider.dart';
import 'package:booky_app/providers/books_provider.dart';
import 'package:booky_app/providers/interests_provider.dart';
import 'package:booky_app/providers/quotations_provider.dart';
import 'package:booky_app/screens/books_explore_screen.dart';
import 'package:booky_app/screens/home_screen.dart';
import 'package:booky_app/screens/interests_screen.dart';
import 'package:booky_app/screens/login_screen.dart';
import 'package:booky_app/screens/splash_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
// Import providers and screens (to be created)

class BookyApp extends StatelessWidget {
  const BookyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (context) => AuthProvider()),
        ChangeNotifierProvider(create: (context) => InterestsProvider()),
        ChangeNotifierProvider(create: (context) => QuotationsProvider()),
        ChangeNotifierProvider(create: (context) => BooksProvider()),
        // Add providers here
      ],
      child: MaterialApp(
        title: 'BookyApp',
        theme: ThemeData(
          primarySwatch: Colors.blue,
        ),
        initialRoute: '/splash',
        routes: {
          '/splash': (context) => const SplashScreen(),
          '/login': (context) => const LoginScreen(),
          '/interests': (context) => const InterestsScreen(),
          '/home': (context) => const HomeScreen(),
          '/books': (context) => const BooksExploreScreen(),
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
