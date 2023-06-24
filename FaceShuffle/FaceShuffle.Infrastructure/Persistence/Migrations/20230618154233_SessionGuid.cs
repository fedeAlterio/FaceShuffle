using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceShuffle.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SessionGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinutesBeforeExpiration",
                table: "UserSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SessionGuid",
                table: "UserSessions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesBeforeExpiration",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "SessionGuid",
                table: "UserSessions");
        }
    }
}
