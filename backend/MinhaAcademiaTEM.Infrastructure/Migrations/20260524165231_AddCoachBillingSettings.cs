using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachBillingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingCurrency",
                table: "Coaches",
                type: "NVARCHAR(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRate",
                table: "Coaches",
                type: "DECIMAL(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingCurrency",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "MonthlyRate",
                table: "Coaches");
        }
    }
}
