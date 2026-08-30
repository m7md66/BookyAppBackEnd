using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDto
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public string? ImageExtention { get; set; }

    }
}
