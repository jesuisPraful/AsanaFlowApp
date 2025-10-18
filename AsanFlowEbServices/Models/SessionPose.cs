using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class SessionPose
    {
        [Key]
        public int SessionPoseId { get; set; }

        [Required]
        public int? SessionId { get; set; }

        [Required]
        public int? PoseId { get; set; }

        [Required]
        public int? Duration { get; set; }
    }
}
