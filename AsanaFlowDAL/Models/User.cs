using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public bool? EmailVerified { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? PasswordLastChanged { get; set; }

    public DateTime? LastLogin { get; set; }

    public int? FailedLoginAttempts { get; set; } = 0;

    public DateTime? AccountLockedUntil { get; set; }

    public bool? MfaEnabled { get; set; }

    public string? MfaSecret { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();

    public virtual ICollection<YogaPose> Poses { get; set; } = new List<YogaPose>();
}
