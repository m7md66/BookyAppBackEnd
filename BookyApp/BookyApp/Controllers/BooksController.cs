using Application.Contracts.Services;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) { _bookService = bookService; }



        [HttpGet]
        public ApiResponse<Book> getFavoriteBooks() {
            //var ss = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;
            var ss = "4e5a47d7-dafd-49fd-b423-16defbb56726";
            var result = _bookService.getFavoriteBooks(ss);
            return result;
        }

    }
}
