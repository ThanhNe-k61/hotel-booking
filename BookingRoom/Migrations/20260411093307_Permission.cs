using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    /// <inheritdoc />
    public partial class Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultRole",
                table: "NguoiDungs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsProfileCompleted",
                table: "NguoiDungs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultRole",
                table: "NguoiDungs");

            migrationBuilder.DropColumn(
                name: "IsProfileCompleted",
                table: "NguoiDungs");
        }
    }
}
