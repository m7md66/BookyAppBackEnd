using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDto
{
    public class UpdateAvatarRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string ImageUrl { get; set; }
    }
}
