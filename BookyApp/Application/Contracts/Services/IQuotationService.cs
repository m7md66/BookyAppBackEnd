using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.QuotationDto;
using Application.DTOs.UserDto;
namespace Application.Contracts.Services
{
    public interface IQuotationService
    {
        Task<ApiResponse<Quotation>> CreateQuotation(CreateQuotation quotationDto);
        Task<ApiResponse<bool>> LikeQuotation(Guid quotationId, string userId);
        Task<ApiResponse<bool>> RequoteQuotation(Guid quotationId, string userId);
        Task<ApiResponse<List<QuotationResponse>>> GetMyQuotation(GetMyQuotationRequest request);
        Task<ApiResponse<bool>> CommentQuotation(CommentQuotationRequest dto);
        Task<ApiResponse<bool>> ShareQuotation(ApiResponse<bool> response,Guid quotationId);
       ApiResponse<List<GetCommentsResponse>> GetQuotationComments(ApiResponse<List<GetCommentsResponse>> response, GetQuotationCommentsRequest request);
        ApiResponse<List<UserResponse>> GetQuotationReQuote(ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request);

        ApiResponse<List<UserResponse>> GetQuotationLikes(ApiResponse<List<UserResponse>> response, GetQuotationCommentsRequest request);





    }
}
