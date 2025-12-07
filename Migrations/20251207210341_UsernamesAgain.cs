using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaAndBeautyWebsite.Migrations
{
    public partial class FixEmployeeUsernames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only touch the Username column if it exists
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    -- Update seeded employees by EmployeeId if username is missing (adjust ids if your data differs)
    UPDATE dbo.Employee
    SET Username = 'sofia.andersson'
    WHERE (Username IS NULL OR Username = '') AND EmployeeId = 1;

    UPDATE dbo.Employee
    SET Username = 'lucas.martinez'
    WHERE (Username IS NULL OR Username = '') AND EmployeeId = 2;

    UPDATE dbo.Employee
    SET Username = 'emma.nguyen'
    WHERE (Username IS NULL OR Username = '') AND EmployeeId = 3;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    UPDATE dbo.Employee
    SET Username = ''
    WHERE EmployeeId IN (1,2,3)
END
");
        }
    }
}