using System.ComponentModel.DataAnnotations;

namespace SpaAndBeautyWebsite.Models
{
    public class BusinessInfo
    {
        [Key]
        [Required]
        public int BusinessInfoId { get; set; }

        [Required]
        [StringLength(100)]
        public required string ItemType { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }
    }
}
