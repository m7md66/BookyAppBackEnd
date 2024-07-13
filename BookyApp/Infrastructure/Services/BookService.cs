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

namespace Infrastructure.Services
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

        public ApiResponse<Book> AddBook(CreateBook createBook)
        {
            var response = new ApiResponse<Book>();
            var book = createBook.Adapt<Book>();
            try
            {
                _bookRepository.Add(book);
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

        public ApiResponse<Book> getBooks(CreateBook createBook)
        {
            var response = new ApiResponse<Book>();
            var book = createBook.Adapt<Book>();
            try
            {
                _bookRepository.Add(book);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }
            response.Status = true;
            return response;
        }

        public ApiResponse<Book> getFavoriteBooks(string UserId) {
            var response = new ApiResponse<Book>();

            var Fboks = _favoriteUserBooksRepository.GetMany(a => a.UserId == UserId).Select(x => new { x.Book}).ToList();
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
            response.DataResult=Fboks;
            response.Status = true;
            return response;
        }
    }
}
