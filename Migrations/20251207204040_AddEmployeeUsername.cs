using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaAndBeautyWebsite.Migrations
{
    public partial class AddEmployeeUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // create Appointment only if it doesn't already exist
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.Appointment','U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Appointment] (
        [AppointmentId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [EmployeeId] INT NOT NULL,
        [CustomerId] INT NOT NULL,
        [ServiceId] INT NOT NULL,
        [ScheduledDateTime] DATETIME2 NOT NULL,
        [DurationMinutes] INT NOT NULL,
        [Status] NVARCHAR(20) NOT NULL
    );
END
");

            // Fix employee rows that have empty or NULL Username values by matching seeded emails.
            // This is defensive: only run updates if the Username column exists.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    -- Update seeded employees by email if username is missing
    UPDATE dbo.Employee
    SET Username = 'sofia.andersson'
    WHERE (Username IS NULL OR Username = '') AND Email = 'sofia.andersson@example.com';

    UPDATE dbo.Employee
    SET Username = 'lucas.martinez'
    WHERE (Username IS NULL OR Username = '') AND Email = 'lucas.martinez@example.com';

    UPDATE dbo.Employee
    SET Username = 'emma.nguyen'
    WHERE (Username IS NULL OR Username = '') AND Email = 'emma.nguyen@example.com';
END
");
            // Note: keep other CreateTable calls (Customer, Employee, Review, Service) handled elsewhere or handled similarly if those may already exist.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.Appointment','U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Appointment];
END
");

            // Optionally revert username fixes when rolling back the migration.
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Employee', 'Username') IS NOT NULL
BEGIN
    -- Revert the specific username fixes back to empty strings if present.
    UPDATE dbo.Employee
    SET Username = ''
    WHERE Email = 'sofia.andersson@example.com' AND Username = 'sofia.andersson';

    UPDATE dbo.Employee
    SET Username = ''
    WHERE Email = 'lucas.martinez@example.com' AND Username = 'lucas.martinez';

    UPDATE dbo.Employee
    SET Username = ''
    WHERE Email = 'emma.nguyen@example.com' AND Username = 'emma.nguyen';
END
");
            // Keep other DropTable calls or handle similarly
        }
    }
}