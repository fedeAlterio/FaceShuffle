using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceShuffle.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TypedUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSessions_Name",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserSessions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_Name",
                table: "UserSessions",
                column: "Name",
                unique: true);
        }
    }
}
