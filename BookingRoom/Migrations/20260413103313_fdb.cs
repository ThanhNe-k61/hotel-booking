using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    /// <inheritdoc />
    public partial class fdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MucLuongCoBan",
                table: "TrucBuongs",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MucLuongCoBan",
                table: "LeTans",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DatPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KhachHangId = table.Column<Guid>(type: "uuid", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NgayNhanPhongDuKien = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NgayTraPhongDuKien = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatPhong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatPhong_KhachHangs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "KhachHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DichVu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenDichVu = table.Column<string>(type: "text", nullable: false),
                    Gia = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LoaiDichVu = table.Column<string>(type: "text", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoaiPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenLoaiPhong = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GiaCoBan = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SoNguoiToiDa = table.Column<int>(type: "integer", nullable: false),
                    AnhDaiDien = table.Column<string>(type: "text", nullable: true),
                    MoTa = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThietBi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenThietBi = table.Column<string>(type: "text", nullable: false),
                    TongSoLuong = table.Column<int>(type: "integer", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DatPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeTanId = table.Column<Guid>(type: "uuid", nullable: true),
                    TongTienPhong = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_HoaDon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoaDon_DatPhong_DatPhongId",
                        column: x => x.DatPhongId,
                        principalTable: "DatPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_LeTans_LeTanId",
                        column: x => x.LeTanId,
                        principalTable: "LeTans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChinhSachGia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoaiPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenChinhSach = table.Column<string>(type: "text", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ApDungChoThu = table.Column<string>(type: "text", nullable: false),
                    LoaiDieuChinh = table.Column<string>(type: "text", nullable: false),
                    GiaTriDieuChinh = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DoUuTien = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChinhSachGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChinhSachGia_LoaiPhong_LoaiPhongId",
                        column: x => x.LoaiPhongId,
                        principalTable: "LoaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoaiPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoPhong = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Tang = table.Column<int>(type: "integer", nullable: false),
                    TrangThai = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phong_LoaiPhong_LoaiPhongId",
                        column: x => x.LoaiPhongId,
                        principalTable: "LoaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDatPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DatPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhongId = table.Column<Guid>(type: "uuid", nullable: true),
                    GiaThucTe = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ThoiGianNhanPhongThucTe = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ThoiGianTraPhongThucTe = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDatPhong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDatPhong_DatPhong_DatPhongId",
                        column: x => x.DatPhongId,
                        principalTable: "DatPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDatPhong_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ThietBiPhong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThietBiId = table.Column<Guid>(type: "uuid", nullable: false),
                    TinhTrang = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBiPhong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThietBiPhong_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThietBiPhong_ThietBi_ThietBiId",
                        column: x => x.ThietBiId,
                        principalTable: "ThietBi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KhachLuuTru",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChiTietDatPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    HoTen = table.Column<string>(type: "text", nullable: false),
                    CCCD_Passport = table.Column<string>(type: "text", nullable: false),
                    QuocTich = table.Column<string>(type: "text", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ThoiGianCheckIn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ThoiGianCheckOut = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachLuuTru", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhachLuuTru_ChiTietDatPhong_ChiTietDatPhongId",
                        column: x => x.ChiTietDatPhongId,
                        principalTable: "ChiTietDatPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuDungDichVu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChiTietDatPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    DichVuId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoLuong = table.Column<int>(type: "integer", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuDungDichVu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuDungDichVu_ChiTietDatPhong_ChiTietDatPhongId",
                        column: x => x.ChiTietDatPhongId,
                        principalTable: "ChiTietDatPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuDungDichVu_DichVu_DichVuId",
                        column: x => x.DichVuId,
                        principalTable: "DichVu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaiSanHuHai",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChiTietDatPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThietBiPhongId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrucBuongId = table.Column<Guid>(type: "uuid", nullable: true),
                    MoTaThietHai = table.Column<string>(type: "text", nullable: false),
                    PhiDenBu = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiSanHuHai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaiSanHuHai_ChiTietDatPhong_ChiTietDatPhongId",
                        column: x => x.ChiTietDatPhongId,
                        principalTable: "ChiTietDatPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaiSanHuHai_ThietBiPhong_ThietBiPhongId",
                        column: x => x.ThietBiPhongId,
                        principalTable: "ThietBiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaiSanHuHai_TrucBuongs_TrucBuongId",
                        column: x => x.TrucBuongId,
                        principalTable: "TrucBuongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChinhSachGia_LoaiPhongId",
                table: "ChinhSachGia",
                column: "LoaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatPhong_DatPhongId",
                table: "ChiTietDatPhong",
                column: "DatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatPhong_PhongId",
                table: "ChiTietDatPhong",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_KhachHangId",
                table: "DatPhong",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_DatPhongId",
                table: "HoaDon",
                column: "DatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_LeTanId",
                table: "HoaDon",
                column: "LeTanId");

            migrationBuilder.CreateIndex(
                name: "IX_KhachLuuTru_ChiTietDatPhongId",
                table: "KhachLuuTru",
                column: "ChiTietDatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_LoaiPhongId",
                table: "Phong",
                column: "LoaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_SoPhong",
                table: "Phong",
                column: "SoPhong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuDungDichVu_ChiTietDatPhongId",
                table: "SuDungDichVu",
                column: "ChiTietDatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_SuDungDichVu_DichVuId",
                table: "SuDungDichVu",
                column: "DichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiSanHuHai_ChiTietDatPhongId",
                table: "TaiSanHuHai",
                column: "ChiTietDatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiSanHuHai_ThietBiPhongId",
                table: "TaiSanHuHai",
                column: "ThietBiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiSanHuHai_TrucBuongId",
                table: "TaiSanHuHai",
                column: "TrucBuongId");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBiPhong_PhongId",
                table: "ThietBiPhong",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBiPhong_ThietBiId",
                table: "ThietBiPhong",
                column: "ThietBiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChinhSachGia");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "KhachLuuTru");

            migrationBuilder.DropTable(
                name: "SuDungDichVu");

            migrationBuilder.DropTable(
                name: "TaiSanHuHai");

            migrationBuilder.DropTable(
                name: "DichVu");

            migrationBuilder.DropTable(
                name: "ChiTietDatPhong");

            migrationBuilder.DropTable(
                name: "ThietBiPhong");

            migrationBuilder.DropTable(
                name: "DatPhong");

            migrationBuilder.DropTable(
                name: "Phong");

            migrationBuilder.DropTable(
                name: "ThietBi");

            migrationBuilder.DropTable(
                name: "LoaiPhong");

            migrationBuilder.AlterColumn<decimal>(
                name: "MucLuongCoBan",
                table: "TrucBuongs",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MucLuongCoBan",
                table: "LeTans",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);
        }
    }
}
