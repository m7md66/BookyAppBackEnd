using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public  class QuotationResponse:BaseDto<QuotationResponse,Quotation>
    {
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }

        //Book data

        public string BookTitle { get; set; }
        public string BookAuther { get; set; }

        public int CommentsNumber { get; set; } = 0;
        public int LikesNumber { get; set; } = 0;
        public int ReQueteNumber { get; set; } = 0;
        public int SharesNumber { get; set; } = 0;



    }
}
