using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_RemoveIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "EquipmentSelections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "EquipmentSelections",
                type: "BIT",
                nullable: false,
                defaultValue: false);
        }
    }
}
