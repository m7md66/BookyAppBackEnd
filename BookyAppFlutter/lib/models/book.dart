class Book {
  final String id;
  final String title;
  final String author;
  final String description;
  final bool isFavorite;

  Book({
    required this.id,
    required this.title,
    required this.author,
    required this.description,
    required this.isFavorite,
  });

  factory Book.fromJson(Map<String, dynamic> json) {
    return Book(
      id: json['id'] ?? '',
      title: json['title'] ?? '',
      author: json['author'] ?? '',
      description: json['description'] ?? '',
      isFavorite: json['isFavorite'] ?? false,
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'title': title,
        'author': author,
        'description': description,
        'isFavorite': isFavorite,
      };
} 