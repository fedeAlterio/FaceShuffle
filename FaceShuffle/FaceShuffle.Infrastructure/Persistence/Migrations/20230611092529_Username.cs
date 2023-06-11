using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceShuffle.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Username : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionGuid",
                table: "UserSessions");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UserSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "UserSessions");

            migrationBuilder.AddColumn<Guid>(
                name: "SessionGuid",
                table: "UserSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
