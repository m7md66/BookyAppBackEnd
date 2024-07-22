using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.QuotationDto;
namespace Application.Contracts.Services
{
    public interface IQuotationService
    {
        ApiResponse<Quotation> CreateQuotation(CreateQuotation quotationDto);
        Task<ApiResponse<Quotation>> LikeQuotation(Guid quotationId, string userId);
        Task<ApiResponse<Quotation>> RequoteQuotation(Guid quotationId, string userId);
        Task<ApiResponse<Quotation>> GetMyQuotation(string userId);
    }
}
