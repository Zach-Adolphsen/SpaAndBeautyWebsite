using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int ReviewId { get; set; }

        [Required]
        [ForeignKey(nameof(EmployeeId))]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey(nameof(CustomerId))]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey(nameof(ServiceId))]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }
    }
}
