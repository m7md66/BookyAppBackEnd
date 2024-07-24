using Application.Contracts.Services;
using Application.DTOs;
using Application.DTOs.BookDto;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) { _bookService = bookService; }



        [HttpGet("GetFavoriteBooks")]
        public ApiResponse<List<BookResponse>> GetFavoriteBooks() {
           
            var result = _bookService.getFavoriteBooks(currentUserId);
            return result;
        }

        [HttpGet("getAllBooks")]
        public Task<ApiResponse<List<BookResponse>>> GetAllBooks()
        {
            var result = _bookService.getBooks();
            return result;
        }
        [HttpPost("AddBook")]
        public ApiResponse<bool> AddBook(CreateBook createBook)
        {
            var result = _bookService.AddBook(createBook);
            return result;
        }

    }
}
