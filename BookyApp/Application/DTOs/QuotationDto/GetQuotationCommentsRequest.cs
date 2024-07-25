using Application.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public class GetQuotationCommentsRequest
    {
        public Guid quotationId { get; set; }
        public PagenationObj pagenation { get; set; }
    }
}
