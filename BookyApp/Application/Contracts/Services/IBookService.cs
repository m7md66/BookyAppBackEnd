using Application.DTOs;
using Application.DTOs.BookDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Services
{
    public interface IBookService
    {
        ApiResponse<List<BookResponse>> getFavoriteBooks(string UserId);
        Task<ApiResponse<List<BookResponse>>> getBooks();
        ApiResponse<bool> AddBook(CreateBook createBook);

    }
}
