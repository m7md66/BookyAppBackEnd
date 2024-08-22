using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Repository;
using Application.DTOs;
using Application.DTOs.BookDto;
using Application.Utility_Classes;
using Domain.Entities;
using Mapster;

namespace Infrastructure.Services
{

    /// <summary>
    ///  hello from the other side
    /// </summary>
    public class QuotationService
    {
        private readonly IBaseRepository<Quotation> _quotationRepository;
        private readonly IBaseRepository<QuotationLike> _QuotationLikeRepository;
        private readonly IBaseRepository<ReQuote> _reQuoteRepository;
        private readonly IBaseRepository<QuotationShare> _quotationShareRepository;
        private readonly IBaseRepository<Comment> _commentRepository;

        public QuotationService(IBaseRepository<Quotation> quotationRepository
            , IBaseRepository<QuotationLike> quotationLikeRepository
            , IBaseRepository<ReQuote> reQuoteLikeRepository
            , IBaseRepository<QuotationShare> quotationShareRepository
            ,IBaseRepository<Comment> commentRepository)
        {
            _quotationRepository = quotationRepository;
            _QuotationLikeRepository = quotationLikeRepository;
            _reQuoteRepository = reQuoteLikeRepository;
            _quotationShareRepository = quotationShareRepository;
            _commentRepository = commentRepository;
        }



        public ApiResponse<Quotation> CreateQuotation(CreateQuotation quotationDto) {

            var response = new ApiResponse<Quotation>();
            var newQuotation = quotationDto.Adapt<Quotation>();
            try
            {
                _quotationRepository.Add(newQuotation);
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

        public async Task<ApiResponse<Quotation>> LikeQuotation(Guid quotationId, string userId)
        {
            var response = new ApiResponse<Quotation>();

            try
            {
                //var isExsist = _QuotationLikeRepository.IsExist(a => a.QuotationId == quotationId && a.UserId == userId);
                var isExsist = await _QuotationLikeRepository.GetAsync(a => a.QuotationId == quotationId && a.UserId == userId);
                if (isExsist is QuotationLike qL)
                    _QuotationLikeRepository.Delete(isExsist);
                else
                    _QuotationLikeRepository.Add(new QuotationLike { QuotationId = quotationId, UserId = userId });
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

        public async Task<ApiResponse<Quotation>> RequoteQuotation(Guid quotationId, string userId)
        {
            var response = new ApiResponse<Quotation>();

            try
            {
                var isRequoted = await _reQuoteRepository.GetAsync(a => a.QuotationId == quotationId && a.UserId == userId);
                if (isRequoted is ReQuote reQuote)
                    _reQuoteRepository.Delete(isRequoted);
                else
                    _reQuoteRepository.Add(new ReQuote { QuotationId = quotationId, UserId = userId });
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

        public async Task<ApiResponse<Quotation>> ShareQuotation(Guid quotationId, string userId)
        {
            var response = new ApiResponse<Quotation>();

            try
            {
                var isShared = await _quotationShareRepository.GetAsync(a => a.QuotationId == quotationId && a.UserId == userId);
                if (isShared is QuotationShare reQuote)
                    _quotationShareRepository.Delete(isShared);
                else
                    _quotationShareRepository.Add(new QuotationShare { QuotationId = quotationId, UserId = userId });
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

        public async Task<ApiResponse<Quotation>> CommentOnQuotation(QuotationCommentRequest commentRequest)
        {
            var response = new ApiResponse<Quotation>();

            try
            {
                var comment = commentRequest.Adapt<Comment>();
               _commentRepository.Add(comment);
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

        public async Task<ApiResponse<Quotation>> GetMyQuotation(string userId)
        {
            var response = new ApiResponse<Quotation>();
            try
            {
                var myQuotation = _quotationRepository.GetMany(a => a.UserId == userId);
                myQuotation.ToPagedResult(1, 20);
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



    }
    //*********************************************************
    public class CreateQuotation : BaseDto<CreateQuotation, Quotation>
    {
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
    }

    public class QuotationCommentRequest
    {
        public string UserId { get; set; }
        public Guid QuotationId { get; set; }
        public string Content { get; set; }
    }
}
