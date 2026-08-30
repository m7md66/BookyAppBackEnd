using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDto
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Password { get; set; }
    }
}
