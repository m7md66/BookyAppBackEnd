using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Contracts.Services;
using Application.DTOs;
using Domain.Entities;
using Application.DTOs.QuotationDto;

namespace BookyApp.Controllers
{
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




        [HttpGet]
        public Task<ApiResponse<Quotation>> GetMyQuotation()
        { var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;

           return _quotationService.GetMyQuotation(userId);
        }

        [HttpPost]
        public ApiResponse<Quotation> CreateQuotation(CreateQuotation quotation)
        {

            return _quotationService.CreateQuotation(quotation);
        }

        [HttpPost("LikeQuotation")]
        
        public Task<ApiResponse<Quotation>> LikeQuotation(Guid QuotationId)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;


            return _quotationService.LikeQuotation(QuotationId, userId);
        }

        [HttpPost("RequoteQuotation")]
        public Task<ApiResponse<Quotation>> RequoteQuotation(Guid QuotationId)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;


            return _quotationService.RequoteQuotation(QuotationId, userId);
        }

    }
    }
