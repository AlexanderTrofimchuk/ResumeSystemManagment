using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeSystemManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesForceIdsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalesForceAccountId",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SalesForceContactId",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesForceAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SalesForceContactId",
                table: "AspNetUsers");
        }
    }
}
