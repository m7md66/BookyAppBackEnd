using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDto
{
    public class ResendOtpRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Email { get; set; }
    }
}
