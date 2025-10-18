using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class UserProgress
{
    public int ProgressId { get; set; }

    public int? UserId { get; set; }

    public int? PoseId { get; set; }

    public int? ProficiencyLevel { get; set; }

    public string? Notes { get; set; }

    public DateTime? LastPracticed { get; set; }

    public virtual YogaPose? Pose { get; set; }

    public virtual User? User { get; set; }
}
