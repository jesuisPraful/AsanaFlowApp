using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class BreathingExercise
{
    public int ExerciseId { get; set; }

    public string? ExerciseName { get; set; }

    public string? Technique { get; set; }

    public int? Duration { get; set; }

    public string? Benefits { get; set; }
}
