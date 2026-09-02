using System.Collections.Generic;

namespace Application.DTOs.UserDto
{
    public class AddUserResponse : BaseResponse
    {
        public AddUserResponse() { }

        public AddUserResponse(int statusCode, bool? isSuccess, string responseMessage)
            : base(statusCode, isSuccess, responseMessage) { }

        /// <summary>
        /// True when the client must collect an OTP code and call ConfirmEmail
        /// before continuing. Mirrors Features:RequireEmailConfirmation.
        /// </summary>
        public bool RequiresEmailConfirmation { get; set; }
    }
}
