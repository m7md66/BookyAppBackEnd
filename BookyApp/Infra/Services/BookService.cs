using Application.Contracts.Repository;
using Application.Contracts.Services;
using Application.DTOs;
using Application.DTOs.BookDto;
using Domain.Entities;
using Infra.Helper.Filters;
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
        private readonly Session _session;
        public BookService(IBaseRepository<Book> bookRepository,
            IBaseRepository<FavoriteUserBooks> favoriteUserBooksRepository,
            Session session
            )
        {
            _bookRepository = bookRepository;
            _favoriteUserBooksRepository = favoriteUserBooksRepository;
            _session = session;

        }

        public async Task<ApiResponse<bool>> AddBook(CreateBook createBook)
        {
            var response = new ApiResponse<bool>();
            var book = createBook.Adapt<Book>();
            try
            {
                _bookRepository.Add(book);
                _favoriteUserBooksRepository.Add(new FavoriteUserBooks { UserId = _session.UserId, Book = book });
                await _bookRepository.SaveChangesAsync();
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
            try
            {
                List<Book> Fboks = _favoriteUserBooksRepository.GetMany(a => a.UserId == UserId).Select(x => x.Book).ToList();
                var bookss = Fboks.Adapt<List<BookResponse>>();
                foreach (var b in bookss) b.IsFavorite = true;
                response.DataResult = bookss;
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

        public async Task<ApiResponse<List<BookResponse>>> GetBrowseBooks()
        {
            var response = new ApiResponse<List<BookResponse>>();
            try
            {
                var books = await _bookRepository.GetManyAsync(b => b.CreatedBy != _session.UserId);
                var favoriteBookIds = _favoriteUserBooksRepository.GetMany(f => f.UserId == _session.UserId).Select(f => f.BookId).ToHashSet();
                var result = books.Adapt<List<BookResponse>>();
                foreach (var b in result) b.IsFavorite = favoriteBookIds.Contains(b.Id);
                response.DataResult = result;
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

        public async Task<ApiResponse<bool>> FavorBook(Guid bookId) {

            var response = new ApiResponse<bool>();

            try
            {
                var alreadyFavorited = await _favoriteUserBooksRepository.GetAsync(f => f.UserId == _session.UserId && f.BookId == bookId);
                if (alreadyFavorited == null)
                {
                    var bookEntity = await _bookRepository.FindById(bookId);
                    if (bookEntity is Book book)
                    {
                        _favoriteUserBooksRepository.Add(new FavoriteUserBooks { UserId = _session.UserId, BookId = bookId });
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }
           await _bookRepository.SaveChangesAsync();
            response.Status = true;
            return response;
        }
    }
}
