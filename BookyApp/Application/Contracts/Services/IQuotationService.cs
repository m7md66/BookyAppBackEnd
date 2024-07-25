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
        Task<ApiResponse<Quotation>> CreateQuotation(CreateQuotation quotationDto);
        Task<ApiResponse<bool>> LikeQuotation(Guid quotationId, string userId);
        Task<ApiResponse<bool>> RequoteQuotation(Guid quotationId, string userId);
        Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation(string userId);
        Task<ApiResponse<bool>> CommentQuotation(CommentQuotationDto dto);
    }
}
