using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int? UserId { get; set; }

    public DateTime? Date { get; set; }

    public int? TotalDuration { get; set; }

    public virtual ICollection<SessionPose> SessionPoses { get; set; } = new List<SessionPose>();

    public virtual User? User { get; set; }
}
