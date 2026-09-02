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
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }

        //Book data

        public string BookTitle { get; set; }
        public string BookAuther { get; set; }

        public int CommentsNumber { get; set; } = 0;
        public int LikesNumber { get; set; } = 0;
        public int ReQueteNumber { get; set; } = 0;
        public int SharesNumber { get; set; } = 0;

        // Quote-repost (requote-with-comment) data — set only when this feed item represents a requote
        public bool IsRequote { get; set; } = false;
        public Guid? OriginalQuotationId { get; set; }
        public string? RequoteComment { get; set; }
        public string? RequoterUserId { get; set; }
        public string? RequoterFullName { get; set; }

        public bool IsLikedByMe { get; set; } = false;
        public bool IsRequotedByMe { get; set; } = false;
        public bool IsSharedByMe { get; set; } = false;

    }
}
