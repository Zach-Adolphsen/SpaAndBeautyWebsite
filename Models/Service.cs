using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Service
    {
        [Key]
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(200)]
        public required string Description { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Range(1, int.MaxValue, ErrorMessage = "Service ID must be a positive integer.")]
        public int DurationMinutes { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal DurationPrice { get; set; }
    }
}
