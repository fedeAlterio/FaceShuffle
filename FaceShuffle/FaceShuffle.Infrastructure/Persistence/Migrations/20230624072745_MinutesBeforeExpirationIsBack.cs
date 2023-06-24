using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceShuffle.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MinutesBeforeExpirationIsBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeExpiration",
                table: "UserSessions");

            migrationBuilder.AddColumn<int>(
                name: "MinutesBeforeExpiration",
                table: "UserSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesBeforeExpiration",
                table: "UserSessions");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeExpiration",
                table: "UserSessions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
