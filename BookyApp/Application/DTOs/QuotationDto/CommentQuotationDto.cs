using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public record CommentQuotationDto(string comment, Guid QuotationId)
    {
    }
}
