using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDto
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public string Code { get; set; }
    }
}
