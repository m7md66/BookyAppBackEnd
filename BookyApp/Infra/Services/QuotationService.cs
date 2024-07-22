using Application.Contracts.Repository;
using Application.DTOs;
using Domain.Entities;
using Mapster;
using Application.UtilityClasses;
using Application.Contracts.Services;
using Application.DTOs.QuotationDto;
namespace Infra.Services
{
    public class QuotationService: IQuotationService
    {
        private readonly IBaseRepository<Quotation> _quotationRepository;
        private readonly IBaseRepository<QuotationLike> _QuotationLikeRepository;
        private readonly IBaseRepository<ReQuote> _reQuoteRepository;

        public QuotationService(IBaseRepository<Quotation> quotationRepository, IBaseRepository<QuotationLike> quotationLikeRepository, IBaseRepository<ReQuote> reQuoteLikeRepository)
        {
            _quotationRepository = quotationRepository;
            _QuotationLikeRepository = quotationLikeRepository;
            _reQuoteRepository = reQuoteLikeRepository;
        }

        public ApiResponse<Quotation> CreateQuotation(CreateQuotation quotationDto)
        {

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

        public async Task<ApiResponse<Quotation>> GetMyQuotation(string userId)
        {
            var response = new ApiResponse<Quotation>();

            try
            {
                var myQuotation = _quotationRepository.GetMany(q => q.UserId == userId);
                var myQuotation1 =await _quotationRepository.GetManyAsync(q => q.UserId == userId);
               var r= myQuotation.ToPagedResult(2, 5);

               var rr= myQuotation1.ToPagedResult(2, 5).Items.OrderBy(a=>a.CreatedDate);
                response.DataResult= rr;
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
   

}
