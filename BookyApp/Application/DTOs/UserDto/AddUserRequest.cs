﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDto
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string ImageExtention { get; set; }


        public string Password { get; set; }
        public string Role { get; set; }


    }
}
