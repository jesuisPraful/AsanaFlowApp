using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class YogaPose
{
    public int PoseId { get; set; }

    public string PoseName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Instructions { get; set; }

    public string? Benefits { get; set; }

    public string? Precautions { get; set; }

    public int? DefaultTime { get; set; }

    public virtual YogaCategory? Category { get; set; }

    public virtual ICollection<SessionPose> SessionPoses { get; set; } = new List<SessionPose>();

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
