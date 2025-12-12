using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaAndBeautyWebsite.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        public int AppointmentId { get; set; }

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
        [Column(TypeName = "datetime2")]
        public DateTime ScheduledDateTime { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive integer.")]
        public int DurationMinutes { get; set; }

        // must be "Scheduled", "Checked In", "Completed", "Cancelled" or "No Show"
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(Scheduled|Checked In|In Progress|Completed|Cancelled|No Show)$", ErrorMessage = 
            "Status must be one of the following: Scheduled, Checked In, Completed, Cancelled, No Show")]

        public required string Status { get; set; }
    }
}
