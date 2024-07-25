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
        public Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation(GetMyQuotationRequest request)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;

           return _quotationService.GetMyQuotation(request);
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
        public Task<ApiResponse<bool>> RequoteQuotation(Guid QuotationId)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;


            return _quotationService.RequoteQuotation(QuotationId, userId);
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
       
        public ApiResponse<List<GetCommentsResponse>> GetComments([FromServices] ApiResponse<List<GetCommentsResponse>> response, GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationComments(response,request);
        }

        [HttpGet("GetReQuotes")]
       
        public ApiResponse<List<UserResponse>> GetReQuotes([FromServices] ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationReQuote(response,request);
        }

        [HttpGet("GetLikes")]
       
        public ApiResponse<List<UserResponse>> GetLikes([FromServices] ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request)
        {
            return _quotationService.GetQuotationLikes(response,request);
        }

    }
    }
