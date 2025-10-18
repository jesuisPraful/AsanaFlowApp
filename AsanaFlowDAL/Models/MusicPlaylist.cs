using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class MusicPlaylist
{
    public int PlaylistId { get; set; }

    public string? Source { get; set; }

    public string? PlaylistName { get; set; }

    public string? Url { get; set; }

    public int? CategoryId { get; set; }

    public virtual YogaCategory? Category { get; set; }
}
