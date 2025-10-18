using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public int? TotalDuration { get; set; }
    }
}
