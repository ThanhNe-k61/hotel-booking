using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    /// <inheritdoc />
    public partial class wow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_DatPhongId",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "NgayThanhToan",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "SoTienDaThanhToan",
                table: "HoaDons");

            migrationBuilder.RenameColumn(
                name: "TongTienPhong",
                table: "HoaDons",
                newName: "TongTienBooking");

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HoaDonId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChiTietDatPhongId = table.Column<Guid>(type: "uuid", nullable: true),
                    LeTanId = table.Column<Guid>(type: "uuid", nullable: true),
                    SoTienPhaiTra = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SoTienDaThanhToan = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "text", nullable: true),
                    TrangThaiThanhToan = table.Column<string>(type: "text", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_ChiTietDatPhongs_ChiTietDatPhongId",
                        column: x => x.ChiTietDatPhongId,
                        principalTable: "ChiTietDatPhongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_HoaDons_HoaDonId",
                        column: x => x.HoaDonId,
                        principalTable: "HoaDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_LeTans_LeTanId",
                        column: x => x.LeTanId,
                        principalTable: "LeTans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_DatPhongId",
                table: "HoaDons",
                column: "DatPhongId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_ChiTietDatPhongId",
                table: "ChiTietHoaDons",
                column: "ChiTietDatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_HoaDonId",
                table: "ChiTietHoaDons",
                column: "HoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_LeTanId",
                table: "ChiTietHoaDons",
                column: "LeTanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons",
                column: "LeTanId",
                principalTable: "LeTans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_DatPhongId",
                table: "HoaDons");

            migrationBuilder.RenameColumn(
                name: "TongTienBooking",
                table: "HoaDons",
                newName: "TongTienPhong");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThanhToan",
                table: "HoaDons",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SoTienDaThanhToan",
                table: "HoaDons",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_DatPhongId",
                table: "HoaDons",
                column: "DatPhongId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons",
                column: "LeTanId",
                principalTable: "LeTans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
