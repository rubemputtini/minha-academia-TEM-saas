using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v7_AddCoachIdToGym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Coaches_CoachId",
                table: "Gyms");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "Gyms",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Coaches_CoachId",
                table: "Gyms",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Coaches_CoachId",
                table: "Gyms");

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "Gyms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Coaches_CoachId",
                table: "Gyms",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }
    }
}