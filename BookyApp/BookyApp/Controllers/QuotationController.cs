using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.Services;
using Application.DTOs;
using Domain.Entities;
using Application.DTOs.QuotationDto;
using Microsoft.AspNetCore.Authorization;

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
        public Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation()
        { var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;

           return _quotationService.GetMyQuotation(userId);
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
        public Task<ApiResponse<bool>> CommentQuotation(CommentQuotationDto dto)
        {
            return _quotationService.CommentQuotation(dto);
        }

    }
    }
