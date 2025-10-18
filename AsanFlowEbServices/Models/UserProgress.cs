using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class UserProgress
    {
        [Key]
        public int ProgressId { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]  
        public int? PoseId { get; set; }

        [Required]  
        public int? ProficiencyLevel { get; set; }

        public string? Notes { get; set; }

        public DateTime? LastPracticed { get; set; }
    }
}
