using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v3_CreateEquipmentAK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentSelections_Equipments_EquipmentId",
                table: "EquipmentSelections");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentSelections_EquipmentId",
                table: "EquipmentSelections");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Equipments_Id_CoachId",
                table: "Equipments",
                columns: new[] { "Id", "CoachId" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSelections_EquipmentId_CoachId",
                table: "EquipmentSelections",
                columns: new[] { "EquipmentId", "CoachId" });

            migrationBuilder.CreateIndex(
                name: "UX_EquipmentSelections_Coach_User_Equipment",
                table: "EquipmentSelections",
                columns: new[] { "CoachId", "UserId", "EquipmentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentSelections_Equipments_EquipmentId_CoachId",
                table: "EquipmentSelections",
                columns: new[] { "EquipmentId", "CoachId" },
                principalTable: "Equipments",
                principalColumns: new[] { "Id", "CoachId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentSelections_Equipments_EquipmentId_CoachId",
                table: "EquipmentSelections");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentSelections_EquipmentId_CoachId",
                table: "EquipmentSelections");

            migrationBuilder.DropIndex(
                name: "UX_EquipmentSelections_Coach_User_Equipment",
                table: "EquipmentSelections");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Equipments_Id_CoachId",
                table: "Equipments");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSelections_EquipmentId",
                table: "EquipmentSelections",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentSelections_Equipments_EquipmentId",
                table: "EquipmentSelections",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
