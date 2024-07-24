using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.BookDto
{
    public class BookResponse:BaseDto<BaseResponse,Book>
    {
        public string Title { get; set; }
        public string Auther { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
