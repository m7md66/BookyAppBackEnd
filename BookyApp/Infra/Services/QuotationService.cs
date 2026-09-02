using Application.Contracts.Repository;
using Application.DTOs;
using Domain.Entities;
using Mapster;
using Application.UtilityClasses;
using Application.Contracts.Services;
using Application.DTOs.QuotationDto;
using Infra.Helper.Filters;
using Microsoft.EntityFrameworkCore;
using Application.DTOs.UserDto;
using Application.DTOs.BookDto;

namespace Infra.Services
{
    public class QuotationService: IQuotationService
    {
        private readonly IBaseRepository<Quotation> _quotationRepository;
        private readonly IBaseRepository<QuotationLike> _QuotationLikeRepository;
        private readonly IBaseRepository<ReQuote> _reQuoteRepository;
        private readonly IBaseRepository<Comment> _commentRepository;
        private readonly IBaseRepository<QuotationShare> _shareRepository;
        private readonly IBaseRepository<UserInterest> _userInterestRepository;
        private readonly Session _session;


        public QuotationService(IBaseRepository<Quotation> quotationRepository
            , IBaseRepository<QuotationLike> quotationLikeRepository
            , IBaseRepository<ReQuote> reQuoteLikeRepository
            , Session session
            , IBaseRepository<Comment> commentRepository
            , IBaseRepository<QuotationShare> shareRepository
            , IBaseRepository<UserInterest> userInterestRepository)
        {
            _quotationRepository = quotationRepository;
            _QuotationLikeRepository = quotationLikeRepository;
            _reQuoteRepository = reQuoteLikeRepository;
            _session = session;
            _commentRepository = commentRepository;
            _shareRepository = shareRepository;
            _userInterestRepository = userInterestRepository;
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
        public async Task<ApiResponse<bool>> RequoteQuotation(Guid quotationId, string userId, string? comment)
        {
            var response = new ApiResponse<bool>();
            var hasComment = !string.IsNullOrWhiteSpace(comment);

            try
            {
                var isRequoted = await _reQuoteRepository.GetAsync(a => a.QuotationId == quotationId && a.UserId == userId);
                if (isRequoted is ReQuote reQuote)
                {
                    if (hasComment)
                        reQuote.Content = comment;
                    else
                        _reQuoteRepository.Delete(isRequoted);
                }
                else
                    _reQuoteRepository.Add(new ReQuote { QuotationId = quotationId, UserId = userId, Content = hasComment ? comment : null });
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
        public async Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation(GetMyQuotationRequest request)
        {
            var response = new ApiResponse<List<QuotationResponse>>();

            try
            {
                var myQuotation = _quotationRepository.GetMany(q => q.UserId == _session.UserId)
                    .Include(q=>q.Book)
                    .Include(q=>q.User)
                    .Include(q=>q. ReQuotes)
                    .Include(q=>q.QuotationLikes)
                    .Include(q=>q.Comments)
                    .Include(q=>q.QuotationShares)
                    .ToList();

              var adaptQuotation = myQuotation.Adapt<List<QuotationResponse>>();
              SetMyLikeAndRequoteFlags(myQuotation, adaptQuotation);
              var pagedQ = adaptQuotation.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);

                response.DataResult = pagedQ;
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

        public async Task<ApiResponse<List<QuotationResponse>>> GetMyLikedQuotations(GetMyQuotationRequest request)
        {
            var response = new ApiResponse<List<QuotationResponse>>();

            try
            {
                var likedQuotations = _QuotationLikeRepository.GetMany(l => l.UserId == _session.UserId)
                    .Include(l => l.Quotation).ThenInclude(q => q.Book)
                    .Include(l => l.Quotation).ThenInclude(q => q.User)
                    .Include(l => l.Quotation).ThenInclude(q => q.ReQuotes)
                    .Include(l => l.Quotation).ThenInclude(q => q.QuotationLikes)
                    .Include(l => l.Quotation).ThenInclude(q => q.Comments)
                    .Include(l => l.Quotation).ThenInclude(q => q.QuotationShares)
                    .OrderByDescending(l => l.CreatedDate)
                    .Select(l => l.Quotation)
                    .ToList();

                var adaptQuotation = likedQuotations.Adapt<List<QuotationResponse>>();
                SetMyLikeAndRequoteFlags(likedQuotations, adaptQuotation);
                var pagedQ = adaptQuotation.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);

                response.DataResult = pagedQ;
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

        public async Task<ApiResponse<List<QuotationResponse>>> GetMyRequotedQuotations(GetMyQuotationRequest request)
        {
            var response = new ApiResponse<List<QuotationResponse>>();

            try
            {
                var myRequotes = _reQuoteRepository.GetMany(r => r.UserId == _session.UserId)
                    .Include(r => r.User)
                    .Include(r => r.Quotation).ThenInclude(q => q.Book)
                    .Include(r => r.Quotation).ThenInclude(q => q.User)
                    .Include(r => r.Quotation).ThenInclude(q => q.ReQuotes)
                    .Include(r => r.Quotation).ThenInclude(q => q.QuotationLikes)
                    .Include(r => r.Quotation).ThenInclude(q => q.Comments)
                    .Include(r => r.Quotation).ThenInclude(q => q.QuotationShares)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToList();

                var result = new List<QuotationResponse>();
                foreach (var r in myRequotes)
                {
                    var originalQuotation = r.Quotation;
                    var dto = originalQuotation.Adapt<QuotationResponse>();
                    dto.Id = r.Id;
                    dto.CreatedDate = r.CreatedDate;
                    dto.IsRequote = true;
                    dto.OriginalQuotationId = originalQuotation.Id;
                    dto.RequoteComment = r.Content;
                    dto.RequoterUserId = r.UserId;
                    dto.RequoterFullName = r.User?.FullName;
                    dto.IsLikedByMe = originalQuotation.QuotationLikes?.Any(l => l.UserId == _session.UserId) ?? false;
                    dto.IsRequotedByMe = true;
                    dto.IsSharedByMe = originalQuotation.QuotationShares?.Any(s => s.UserId == _session.UserId) ?? false;
                    result.Add(dto);
                }

                var pagedQ = result.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);

                response.DataResult = pagedQ;
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

        private void SetMyLikeAndRequoteFlags(List<Quotation> source, List<QuotationResponse> adapted)
        {
            for (int i = 0; i < source.Count; i++)
            {
                adapted[i].IsLikedByMe = source[i].QuotationLikes?.Any(l => l.UserId == _session.UserId) ?? false;
                adapted[i].IsRequotedByMe = source[i].ReQuotes?.Any(r => r.UserId == _session.UserId) ?? false;
                adapted[i].IsSharedByMe = source[i].QuotationShares?.Any(s => s.UserId == _session.UserId) ?? false;
            }
        }
        public async Task<ApiResponse<bool>> CommentQuotation(CommentQuotationRequest dto)
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

        public async Task<ApiResponse<bool>> ShareQuotation(ApiResponse<bool> response, Guid quotationId)
        {
            bool isSharedNow;
            try
            {
                var isShared = await _shareRepository.GetAsync(a => a.QuotationId == quotationId && a.UserId == _session.UserId);
                if (isShared is QuotationShare shared)
                {
                    _shareRepository.Delete(isShared);
                    isSharedNow = false;
                }
                else
                {
                    _shareRepository.Add(new QuotationShare { QuotationId = quotationId, UserId = _session.UserId });
                    isSharedNow = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response.Errors.Add(ex.ToString());
                response.Status = false;
                return response;

            }
            await _shareRepository.SaveChangesAsync();
            response.Status = true;
            response.Data = isSharedNow;
            return response;
        }

        public ApiResponse<List<GetCommentsResponse>> GetQuotationComments(ApiResponse<List<GetCommentsResponse>> response, GetQuotationCommentsRequest request)
        {
            try
            {
                var comments = _commentRepository
                    .GetMany(a => a.QuotationId == request.quotationId)
                    .Select(a=>new GetCommentsResponse {Content=a.Content,User=new UserResponse {FirstName=a.user.FirstName,LastName=a.user.LastName,ImageUrl=a.user.ImageUrl,ImageExtention=a.user.ImageExtention} })
                    .ToList();
                var commentPaged=comments.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);
                response.DataResult = commentPaged;

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

        public ApiResponse<List<UserResponse>> GetQuotationReQuote(ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request)
        {
            try
            {
                var reQuote = _reQuoteRepository
                    .GetMany(a => a.QuotationId == request.quotationId)
                    .Select(a =>new UserResponse { FirstName = a.User.FirstName, LastName = a.User.LastName, ImageUrl = a.User.ImageUrl, ImageExtention = a.User.ImageExtention } )
                    .ToList();
                var reQuotePaged = reQuote.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);
                response.DataResult = reQuotePaged;

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



        public ApiResponse<List<UserResponse>> GetQuotationLikes(ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request)
        {
            try
            {
                var reQuote = _QuotationLikeRepository
                    .GetMany(a => a.QuotationId == request.quotationId)
                    .Select(a => new UserResponse { FirstName = a.User.FirstName, LastName = a.User.LastName, ImageUrl = a.User.ImageUrl, ImageExtention = a.User.ImageExtention })
                    .ToList();
                var reQuotePaged = reQuote.ToPagedResult(request.pagenation.pageNumber, request.pagenation.pageSize);
                response.DataResult = reQuotePaged;

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







        public async Task<ApiResponse<List<QuotationResponse>>> GetFeed(GetFeedRequest request)
        {
            var response = new ApiResponse<List<QuotationResponse>>();
            try
            {
                var userInterestIds = _userInterestRepository
                    .GetMany(a => a.UserId == _session.UserId)
                    .Select(a => a.InterestId)
                    .ToList();

                var quotations = _quotationRepository
                    .GetMany(q => true)
                    .Include(q => q.Book)
                        .ThenInclude(b => b.BookGenres)
                    .Include(q => q.User)
                    .Include(q => q.ReQuotes)
                    .Include(q => q.QuotationLikes)
                    .Include(q => q.Comments)
                    .Include(q => q.QuotationShares)
                    .ToList();

                var quotationsById = quotations.ToDictionary(q => q.Id);

                var requotesWithComment = _reQuoteRepository
                    .GetMany(r => r.Content != null && r.Content != "")
                    .Include(r => r.User)
                    .ToList();

                var feedItems = new List<(QuotationResponse Item, bool MatchesInterest, DateTime? SortDate)>();

                foreach (var q in quotations)
                {
                    var dto = q.Adapt<QuotationResponse>();
                    dto.IsLikedByMe = q.QuotationLikes?.Any(l => l.UserId == _session.UserId) ?? false;
                    dto.IsRequotedByMe = q.ReQuotes?.Any(r => r.UserId == _session.UserId) ?? false;
                    dto.IsSharedByMe = q.QuotationShares?.Any(s => s.UserId == _session.UserId) ?? false;
                    var matches = q.Book?.BookGenres?.Any(bg => userInterestIds.Contains(bg.GenrId)) ?? false;
                    feedItems.Add((dto, matches, q.CreatedDate));
                }

                foreach (var r in requotesWithComment)
                {
                    if (!quotationsById.TryGetValue(r.QuotationId, out var originalQuotation))
                        continue;

                    var dto = originalQuotation.Adapt<QuotationResponse>();
                    dto.Id = r.Id;
                    dto.CreatedDate = r.CreatedDate;
                    dto.IsRequote = true;
                    dto.OriginalQuotationId = originalQuotation.Id;
                    dto.RequoteComment = r.Content;
                    dto.RequoterUserId = r.UserId;
                    dto.RequoterFullName = r.User?.FullName;
                    dto.IsLikedByMe = originalQuotation.QuotationLikes?.Any(l => l.UserId == _session.UserId) ?? false;
                    dto.IsRequotedByMe = originalQuotation.ReQuotes?.Any(l => l.UserId == _session.UserId) ?? false;
                    dto.IsSharedByMe = originalQuotation.QuotationShares?.Any(s => s.UserId == _session.UserId) ?? false;
                    var matches = originalQuotation.Book?.BookGenres?.Any(bg => userInterestIds.Contains(bg.GenrId)) ?? false;
                    feedItems.Add((dto, matches, r.CreatedDate));
                }

                var ordered = feedItems
                    .OrderByDescending(x => x.MatchesInterest)
                    .ThenByDescending(x => x.SortDate)
                    .Select(x => x.Item)
                    .ToList();

                response.DataResult = ordered.ToPagedResult(request.Pagenation.pageNumber, request.Pagenation.pageSize);
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

        public async Task<ApiResponse<QuotationResponse>> GetQuotationById(Guid quotationId)
        {
            var response = new ApiResponse<QuotationResponse>();
            try
            {
                var quotation = _quotationRepository
                    .GetMany(q => q.Id == quotationId)
                    .Include(q => q.Book)
                    .Include(q => q.User)
                    .Include(q => q.ReQuotes)
                    .Include(q => q.QuotationLikes)
                    .Include(q => q.Comments)
                    .Include(q => q.QuotationShares)
                    .FirstOrDefault();

                if (quotation is null)
                {
                    response.Status = false;
                    response.Message = "Quotation not found";
                    return response;
                }

                var dto = quotation.Adapt<QuotationResponse>();
                // _session.UserId is null for anonymous callers (e.g. the public /q/{id} page) — flags stay false.
                dto.IsLikedByMe = quotation.QuotationLikes?.Any(l => l.UserId == _session.UserId) ?? false;
                dto.IsRequotedByMe = quotation.ReQuotes?.Any(r => r.UserId == _session.UserId) ?? false;
                dto.IsSharedByMe = quotation.QuotationShares?.Any(s => s.UserId == _session.UserId) ?? false;
                response.Data = dto;
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
}
