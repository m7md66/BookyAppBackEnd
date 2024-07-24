using Application.Contracts.Repository;
using Application.Contracts.Services;
using Application.DTOs;
using Application.DTOs.BookDto;
using Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class BookService: IBookService
    {
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IBaseRepository<FavoriteUserBooks> _favoriteUserBooksRepository;
        public BookService(IBaseRepository<Book> bookRepository,
            IBaseRepository<FavoriteUserBooks> favoriteUserBooksRepository
            )
        {
            _bookRepository = bookRepository;
            _favoriteUserBooksRepository = favoriteUserBooksRepository;
        }

        public ApiResponse<bool> AddBook(CreateBook createBook)
        {
            var response = new ApiResponse<bool>();
            var book = createBook.Adapt<Book>();
            try
            {
                _bookRepository.Add(book);
                _bookRepository.SaveChangesAsync();
            }
            catch (Exception ex) { 

            Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status=false;
                return response;

            }
            response.Status = true  ;
            return response;
        }

        public async Task<ApiResponse<List<BookResponse>>> getBooks()
        {
            var response = new ApiResponse<List<BookResponse>>();
            var Fboks =await _bookRepository.GetAllAsync();
            var bookss = Fboks.Adapt<List<BookResponse>>();
            try
            {
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }
            response.Data = bookss;
            response.Status = true;
            return response;
        }

        public ApiResponse<List<BookResponse>> getFavoriteBooks(string UserId) {
            var response = new ApiResponse<List<BookResponse>>();
            var Fboks = _favoriteUserBooksRepository.GetMany(a => a.UserId == UserId).Select(x => new { x.Book}).ToList();
            var bookss = Fboks.Adapt<List<BookResponse>>();
            try
            {
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }
            response.DataResult=bookss;
            response.Status = true;
            return response;
        }
    }
}
