class Quotation {
  final String id;
  final String text;
  final String author;
  final int likes;
  final int comments;
  final bool isLiked;

  Quotation({
    required this.id,
    required this.text,
    required this.author,
    required this.likes,
    required this.comments,
    required this.isLiked,
  });

  factory Quotation.fromJson(Map<String, dynamic> json) {
    return Quotation(
      id: json['id'] ?? '',
      text: json['text'] ?? '',
      author: json['author'] ?? '',
      likes: json['likes'] ?? 0,
      comments: json['comments'] ?? 0,
      isLiked: json['isLiked'] ?? false,
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'text': text,
        'author': author,
        'likes': likes,
        'comments': comments,
        'isLiked': isLiked,
      };
} 