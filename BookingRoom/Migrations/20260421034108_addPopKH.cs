using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    /// <inheritdoc />
    public partial class addPopKH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "KhachHangs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "KhachHangs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHangs",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueQuan",
                table: "KhachHangs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "KhachHangs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "QueQuan",
                table: "KhachHangs");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "KhachHangs");
        }
    }
}
