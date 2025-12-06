using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(30)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public required string Username { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        // Format: +1-234-567-8901 or 123-456-7890
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^(\+\d{1}-)?\d{3}-\d{3}-\d{4}$", ErrorMessage =
            "Phone number must be in the format +1-234-567-8901 or 123-456-7890")]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
