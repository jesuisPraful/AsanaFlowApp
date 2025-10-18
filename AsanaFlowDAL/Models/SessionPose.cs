using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class SessionPose
{
    public int SessionPoseId { get; set; }

    public int? SessionId { get; set; }

    public int? PoseId { get; set; }

    public int? Duration { get; set; }

    public virtual YogaPose? Pose { get; set; }

    public virtual Session? Session { get; set; }
}
