using System;
using System.Collections.Generic;

namespace AsanaFlowDataAccessLayer.Models;

public partial class YogaCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MusicPlaylist> MusicPlaylists { get; set; } = new List<MusicPlaylist>();

    public virtual ICollection<YogaPose> YogaPoses { get; set; } = new List<YogaPose>();
}
