using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(30)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public required string LastName { get; set; }

        // Format: +1-123-456-7890
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^\+\d{1}-\d{3}-\d{3}-\d{4}$", ErrorMessage = 
            "Phone number must be in the format +1-123-456-7890")]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required] 
        [StringLength(20)]
        [PasswordPropertyText]
        public required string Password { get; set; }

        [Required]
        [StringLength(30)]
        public required string Street { get; set; }

        [Required]
        [StringLength(30)]
        public required string City { get; set; }

        [Required]
        [StringLength(30)]
        public required string State { get; set; }

        [Required]
        [StringLength(15)]
        [RegularExpression(@"^(\+\d{1}-)?\d{3}-\d{3}-\d{4}$", ErrorMessage = 
            "Phone number must be in the format +1-234-567-8901 or 123-456-7890")]
        public required string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public required string JobTitle { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        public decimal Salary { get; set; }

        // must be "Admin", "Staff", or "Manager"
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^(Admin|Staff|Manager)$", ErrorMessage = 
            "Permission must be either 'Admin', 'Staff', or 'Manager'")]
        public required string Permission { get; set; }
    }
}