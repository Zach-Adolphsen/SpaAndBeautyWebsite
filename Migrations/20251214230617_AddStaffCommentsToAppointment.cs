using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaAndBeautyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffCommentsToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StaffComments",
                table: "Appointment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffComments",
                table: "Appointment");
        }
    }
}
