using Application.Contracts.Services;
using Application.DTOs;
using Application.DTOs.BookDto;
using Domain.Entities;
using Infra.Helper.Filters;
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
        private readonly Session _session;
        public BooksController(IBookService bookService,Session session) { _bookService = bookService; _session = session; }



        [HttpGet("GetFavoriteBooks")]
        public ApiResponse<List<BookResponse>> GetFavoriteBooks() {
           
            var result = _bookService.getFavoriteBooks(_session.UserId);
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

        [HttpPost("FavorBook")]
        public async Task<ApiResponse<bool>> FavBook(Guid bookId)
        {
            var result =await _bookService.FavorBook(bookId);
            return result;
        }


    }
}
