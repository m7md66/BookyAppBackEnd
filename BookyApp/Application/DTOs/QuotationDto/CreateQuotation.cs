using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public class CreateQuotation 
    {
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
    }

}
