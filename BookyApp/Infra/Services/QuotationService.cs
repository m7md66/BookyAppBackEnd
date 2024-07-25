using Application.Contracts.Repository;
using Application.DTOs;
using Domain.Entities;
using Mapster;
using Application.UtilityClasses;
using Application.Contracts.Services;
using Application.DTOs.QuotationDto;
using Infra.Helper.Filters;
namespace Infra.Services
{
    public class QuotationService: IQuotationService
    {
        private readonly IBaseRepository<Quotation> _quotationRepository;
        private readonly IBaseRepository<QuotationLike> _QuotationLikeRepository;
        private readonly IBaseRepository<ReQuote> _reQuoteRepository;
        private readonly IBaseRepository<Comment> _commentRepository;
        private readonly Session _session;

        public QuotationService(IBaseRepository<Quotation> quotationRepository
            , IBaseRepository<QuotationLike> quotationLikeRepository
            , IBaseRepository<ReQuote> reQuoteLikeRepository
            ,Session session
            , IBaseRepository<Comment> commentRepository)
        {
            _quotationRepository = quotationRepository;
            _QuotationLikeRepository = quotationLikeRepository;
            _reQuoteRepository = reQuoteLikeRepository;
            _session = session;
            _commentRepository = commentRepository;
        }

        public async Task<ApiResponse<Quotation>> CreateQuotation(CreateQuotation quotationDto)
        {
            var response = new ApiResponse<Quotation>();
            var newQuotation = quotationDto.Adapt<Quotation>();
            newQuotation.UserId = _session.UserId;
            try
            {
                _quotationRepository.Add(newQuotation);
               await _quotationRepository.SaveChangesAsync();
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

        public async Task<ApiResponse<bool>> LikeQuotation(Guid quotationId, string userId)
        {
            var response = new ApiResponse<bool>();

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
            await _QuotationLikeRepository.SaveChangesAsync();
            response.Status = true;
            return response;
        }

        public async Task<ApiResponse<bool>> RequoteQuotation(Guid quotationId, string userId)
        {
            var response = new ApiResponse<bool>();

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
            await _reQuoteRepository.SaveChangesAsync();
            response.Status = true;
            return response;
        }

        public async Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation(string userId)
        {
            var response = new ApiResponse<List<QuotationResponse>>();

            try
            {
                var myQuotation = _quotationRepository.GetMany(q => q.UserId == userId);
                var myQuotation1 =await _quotationRepository.GetManyAsync(q => q.UserId == userId);
               var r= myQuotation.ToPagedResult(2, 5);

               var rr= myQuotation1.ToPagedResult(1, 5).Items.OrderBy(a=>a.CreatedDate).ToList();
                response.DataResult= r;

                response.Data= rr.Adapt<List<QuotationResponse>>();
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





        public async Task<ApiResponse<bool>> CommentQuotation(CommentQuotationDto dto)
        {
            var response = new ApiResponse<bool>();

            try
            {
                _commentRepository.Add(new Comment { UserId = _session.UserId, Content = dto.comment, QuotationId = dto.QuotationId });
                await _commentRepository.SaveChangesAsync();
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
