using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? PasswordLastChanged { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? FailedLoginAttempts { get; set; }

        public DateTime? AccountLockedUntil { get; set; }

        public bool? MfaEnabled { get; set; }

        public string? MfaSecret { get; set; }
    }
}
