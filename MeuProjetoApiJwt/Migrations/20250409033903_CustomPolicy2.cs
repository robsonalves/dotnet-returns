﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuProjetoApiJwt.Migrations
{
    /// <inheritdoc />
    public partial class CustomPolicy2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDelete",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDelete",
                table: "Users");
        }
    }
}
