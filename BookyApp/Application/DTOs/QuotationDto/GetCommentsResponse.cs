using Application.DTOs.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.QuotationDto
{
    public class GetCommentsResponse
    {
        public string Content { get; set; }
        public UserResponse User { get; set; }

    }
}
