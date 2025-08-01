﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaAcademiaTEM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v4_RemoveUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "NVARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
