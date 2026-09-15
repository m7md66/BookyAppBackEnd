using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.Services;
using Application.DTOs;
using Domain.Entities;
using Application.DTOs.QuotationDto;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.UserDto;

namespace BookyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : BaseController
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService) {
            _quotationService = quotationService;
        }
        //var ss = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;

        //GetMyQuotation RequoteQuotation LikeQuotation CreateQuotation




        [HttpGet("GetMyQuotation")]
        public Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation([FromQuery] GetMyQuotationRequest request)
        {
            return _quotationService.GetMyQuotation(request);
        }

        [HttpGet("GetMyLikedQuotations")]
        public Task<ApiResponse<List<QuotationResponse>>> GetMyLikedQuotations([FromQuery] GetMyQuotationRequest request)
        {
            return _quotationService.GetMyLikedQuotations(request);
        }

        [HttpGet("GetMyRequotedQuotations")]
        public Task<ApiResponse<List<QuotationResponse>>> GetMyRequotedQuotations([FromQuery] GetMyQuotationRequest request)
        {
            return _quotationService.GetMyRequotedQuotations(request);
        }

        [HttpPost("CreateQuotation")]
        public Task<ApiResponse<Quotation>> CreateQuotation(CreateQuotation quotation)
        {
            return _quotationService.CreateQuotation(quotation);
        }

        [HttpPost("LikeQuotation")]
        
        public Task<ApiResponse<bool>> LikeQuotation(Guid QuotationId)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;


            return _quotationService.LikeQuotation(QuotationId, userId);
        }

        [HttpPost("RequoteQuotation")]
        public Task<ApiResponse<bool>> RequoteQuotation(RequoteQuotationRequest request)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;


            return _quotationService.RequoteQuotation(request.QuotationId, userId, request.Comment);
        }

        [HttpPost("CommentQuotation")]
        public Task<ApiResponse<bool>> CommentQuotation(CommentQuotationRequest dto)
        {
            return _quotationService.CommentQuotation(dto);
        }

        [HttpPost("ShareQuotation")]
        public Task<ApiResponse<bool>> ShareQuotation([FromServices] ApiResponse<bool> response, Guid QuotationId)
        {
            return _quotationService.ShareQuotation(response,QuotationId);
        }

        [HttpGet("GetComments")]

        public ApiResponse<List<GetCommentsResponse>> GetComments([FromServices] ApiResponse<List<GetCommentsResponse>> response, [FromQuery] GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationComments(response,request);
        }

        [HttpGet("GetReQuotes")]

        public ApiResponse<List<UserResponse>> GetReQuotes([FromServices] ApiResponse<List<UserResponse>> response, [FromQuery] GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationReQuote(response,request);
        }

        [HttpGet("GetLikes")]

        public ApiResponse<List<UserResponse>> GetLikes([FromServices] ApiResponse<List<UserResponse>> response, [FromQuery] GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationLikes(response,request);
        }

        [HttpGet("GetFeed")]
        public Task<ApiResponse<List<QuotationResponse>>> GetFeed([FromQuery] GetFeedRequest request)
        {
            return _quotationService.GetFeed(request);
        }

        [HttpGet("GetQuotation")]
        public Task<ApiResponse<QuotationResponse>> GetQuotation(Guid id)
        {
            return _quotationService.GetQuotationById(id);
        }

    }
    }
