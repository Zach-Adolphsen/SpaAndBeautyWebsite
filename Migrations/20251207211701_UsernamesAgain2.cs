using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaAndBeautyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class UsernamesAgain2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Populate missing Employee.Username values using the email local-part.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    UPDATE dbo.Employee
    SET Username = LEFT(Email, CHARINDEX('@', Email) - 1)
    WHERE (Username IS NULL OR Username = '') AND Email IS NOT NULL AND CHARINDEX('@', Email) > 1;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert only the rows that were set from Email (optional)
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    UPDATE dbo.Employee
    SET Username = ''
    WHERE Email IS NOT NULL AND Username = LEFT(Email, CHARINDEX('@', Email) - 1);
END
");
        }
        }
}
