using Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// A one-time 6-digit code emailed to a user to confirm their email address
    /// right after registration. Only created when Features:RequireEmailConfirmation is on.
    /// </summary>
    public class EmailOtp : EntityBase
    {
        public string UserId { get; set; }

        /// <summary>SHA-256 hash of the 6-digit code (the plain code is only sent by email).</summary>
        public string CodeHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int Attempts { get; set; }

        public DateTime? ConsumedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser user { get; set; }
    }
}
