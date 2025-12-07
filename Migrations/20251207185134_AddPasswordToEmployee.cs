using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaAndBeautyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only add the column if it does not already exist (prevents duplicate column errors
            // when another migration already created the Password column).
            migrationBuilder.Sql(@"
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE Name = N'Password' AND Object_ID = Object_ID(N'dbo.Employee')
)
BEGIN
    ALTER TABLE [dbo].[Employee] ADD [Password] NVARCHAR(30) NOT NULL CONSTRAINT DF_Employee_Password DEFAULT ('')
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the column only if it exists.
            migrationBuilder.Sql(@"
IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE Name = N'Password' AND Object_ID = Object_ID(N'dbo.Employee')
)
BEGIN
    ALTER TABLE [dbo].[Employee] DROP COLUMN [Password]
END
");
        }
    }
}