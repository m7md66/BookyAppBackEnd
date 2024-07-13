﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Application.DTOs.BookDto
{
    public class CreateBook:BaseDto<CreateBook,Book>
    {
        public string Title { get; set; }
        public string Auther { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
