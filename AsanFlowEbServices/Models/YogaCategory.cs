using System.ComponentModel.DataAnnotations;

namespace AsanaFlowWebServices.Models
{
    public class YogaCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = null!;

        [Required]
        public string? Description { get; set; }
    }
}
