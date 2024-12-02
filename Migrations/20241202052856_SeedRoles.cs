using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert default roles into AspNetRoles table
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { Guid.NewGuid().ToString(), "Admin", "ADMIN" },
                    { Guid.NewGuid().ToString(), "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove default roles if rolling back
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Name",
                keyValue: "Admin");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Name",
                keyValue: "User");
        }
    }
}