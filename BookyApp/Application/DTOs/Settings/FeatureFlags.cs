namespace Application.DTOs.Settings
{
    /// <summary>
    /// Toggles for behaviour that should only run in some environments.
    /// Bound from the "Features" section of appsettings*.json.
    /// </summary>
    public class FeatureFlags
    {
        /// <summary>
        /// When true, a new user must confirm their email with an OTP code
        /// (sent on registration) before the mobile app lets them continue.
        /// Kept false in Development so registration is not interrupted.
        /// </summary>
        public bool RequireEmailConfirmation { get; set; }
    }
}
