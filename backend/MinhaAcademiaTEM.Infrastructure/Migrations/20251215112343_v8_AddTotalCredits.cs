using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v8_AddTotalCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCreditsEarned",
                table: "ReferralAccounts",
                type: "INT",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCreditsEarned",
                table: "ReferralAccounts");
        }
    }
}
