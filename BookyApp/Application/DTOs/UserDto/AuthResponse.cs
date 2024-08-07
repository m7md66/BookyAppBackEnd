﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDto
{
    public class AuthResponse : BaseResponse
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? ExpiresOn { get; set; }
    }
}
