using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        // --- Foreign Keys (The Numbers) ---
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(Pending|Approved|Rejected)$", ErrorMessage = "Status must be: Pending, Approved, or Rejected")]
        public string Status { get; set; } = "Pending";

        [StringLength(500)]
        public string? ManagerComments { get; set; }


        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}