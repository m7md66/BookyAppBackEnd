namespace Application.Localization
{
    /// <summary>
    /// Keys into the shared localization resource (<see cref="Application.SharedResource"/>).
    /// Identity error messages are looked up by "Identity_" + IdentityError.Code.
    /// </summary>
    public static class MessageKeys
    {
        public const string EmailAlreadyRegistered = "EmailAlreadyRegistered";
        public const string UserAddedSuccessfully = "UserAddedSuccessfully";
        public const string InvalidCredentials = "InvalidCredentials";
        public const string GenericError = "GenericError";
        public const string InterestIdsEmpty = "InterestIdsEmpty";
        public const string NoFileSelected = "NoFileSelected";
    }
}
