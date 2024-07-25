using Application.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public class GetMyQuotationRequest
    {
        public string UserId { get; set; }
        public PagenationObj pagenation { get; set; }
    }
}
