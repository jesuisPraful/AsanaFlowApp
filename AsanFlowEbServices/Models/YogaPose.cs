using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class YogaPose
    {

        [Key]
        public int PoseId { get; set; }

        [Required]
        public string PoseName { get; set; } = null!;

        [Required]
        public int? CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string? Instructions { get; set; }

        [Required]
        public string? Benefits { get; set; }

        [Required]  
        public string? Precautions { get; set; }

        [Required]
        public int? DefaultTime { get; set; }
    }
}
