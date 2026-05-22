using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    /// <inheritdoc />
    public partial class InitAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChinhSachGia_LoaiPhong_LoaiPhongId",
                table: "ChinhSachGia");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatPhong_DatPhong_DatPhongId",
                table: "ChiTietDatPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatPhong_Phong_PhongId",
                table: "ChiTietDatPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhong_KhachHangs_KhachHangId",
                table: "DatPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_DatPhong_DatPhongId",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_LeTans_LeTanId",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachLuuTru_ChiTietDatPhong_ChiTietDatPhongId",
                table: "KhachLuuTru");

            migrationBuilder.DropForeignKey(
                name: "FK_Phong_LoaiPhong_LoaiPhongId",
                table: "Phong");

            migrationBuilder.DropForeignKey(
                name: "FK_SuDungDichVu_ChiTietDatPhong_ChiTietDatPhongId",
                table: "SuDungDichVu");

            migrationBuilder.DropForeignKey(
                name: "FK_SuDungDichVu_DichVu_DichVuId",
                table: "SuDungDichVu");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHai_ChiTietDatPhong_ChiTietDatPhongId",
                table: "TaiSanHuHai");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHai_ThietBiPhong_ThietBiPhongId",
                table: "TaiSanHuHai");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHai_TrucBuongs_TrucBuongId",
                table: "TaiSanHuHai");

            migrationBuilder.DropForeignKey(
                name: "FK_ThietBiPhong_Phong_PhongId",
                table: "ThietBiPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_ThietBiPhong_ThietBi_ThietBiId",
                table: "ThietBiPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThietBiPhong",
                table: "ThietBiPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThietBi",
                table: "ThietBi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaiSanHuHai",
                table: "TaiSanHuHai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuDungDichVu",
                table: "SuDungDichVu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Phong",
                table: "Phong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiPhong",
                table: "LoaiPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhachLuuTru",
                table: "KhachLuuTru");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DichVu",
                table: "DichVu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatPhong",
                table: "DatPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDatPhong",
                table: "ChiTietDatPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChinhSachGia",
                table: "ChinhSachGia");

            migrationBuilder.RenameTable(
                name: "ThietBiPhong",
                newName: "ThietBiPhongs");

            migrationBuilder.RenameTable(
                name: "ThietBi",
                newName: "ThietBis");

            migrationBuilder.RenameTable(
                name: "TaiSanHuHai",
                newName: "TaiSanHuHais");

            migrationBuilder.RenameTable(
                name: "SuDungDichVu",
                newName: "SuDungDichVus");

            migrationBuilder.RenameTable(
                name: "Phong",
                newName: "Phongs");

            migrationBuilder.RenameTable(
                name: "LoaiPhong",
                newName: "LoaiPhongs");

            migrationBuilder.RenameTable(
                name: "KhachLuuTru",
                newName: "KhachLuuTrus");

            migrationBuilder.RenameTable(
                name: "HoaDon",
                newName: "HoaDons");

            migrationBuilder.RenameTable(
                name: "DichVu",
                newName: "DichVus");

            migrationBuilder.RenameTable(
                name: "DatPhong",
                newName: "DatPhongs");

            migrationBuilder.RenameTable(
                name: "ChiTietDatPhong",
                newName: "ChiTietDatPhongs");

            migrationBuilder.RenameTable(
                name: "ChinhSachGia",
                newName: "ChinhSachGias");

            migrationBuilder.RenameIndex(
                name: "IX_ThietBiPhong_ThietBiId",
                table: "ThietBiPhongs",
                newName: "IX_ThietBiPhongs_ThietBiId");

            migrationBuilder.RenameIndex(
                name: "IX_ThietBiPhong_PhongId",
                table: "ThietBiPhongs",
                newName: "IX_ThietBiPhongs_PhongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHai_TrucBuongId",
                table: "TaiSanHuHais",
                newName: "IX_TaiSanHuHais_TrucBuongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHai_ThietBiPhongId",
                table: "TaiSanHuHais",
                newName: "IX_TaiSanHuHais_ThietBiPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHai_ChiTietDatPhongId",
                table: "TaiSanHuHais",
                newName: "IX_TaiSanHuHais_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_SuDungDichVu_DichVuId",
                table: "SuDungDichVus",
                newName: "IX_SuDungDichVus_DichVuId");

            migrationBuilder.RenameIndex(
                name: "IX_SuDungDichVu_ChiTietDatPhongId",
                table: "SuDungDichVus",
                newName: "IX_SuDungDichVus_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_Phong_SoPhong",
                table: "Phongs",
                newName: "IX_Phongs_SoPhong");

            migrationBuilder.RenameIndex(
                name: "IX_Phong_LoaiPhongId",
                table: "Phongs",
                newName: "IX_Phongs_LoaiPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_KhachLuuTru_ChiTietDatPhongId",
                table: "KhachLuuTrus",
                newName: "IX_KhachLuuTrus_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_LeTanId",
                table: "HoaDons",
                newName: "IX_HoaDons_LeTanId");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_DatPhongId",
                table: "HoaDons",
                newName: "IX_HoaDons_DatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhong_KhachHangId",
                table: "DatPhongs",
                newName: "IX_DatPhongs_KhachHangId");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatPhong_PhongId",
                table: "ChiTietDatPhongs",
                newName: "IX_ChiTietDatPhongs_PhongId");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatPhong_DatPhongId",
                table: "ChiTietDatPhongs",
                newName: "IX_ChiTietDatPhongs_DatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_ChinhSachGia_LoaiPhongId",
                table: "ChinhSachGias",
                newName: "IX_ChinhSachGias_LoaiPhongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThietBiPhongs",
                table: "ThietBiPhongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThietBis",
                table: "ThietBis",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaiSanHuHais",
                table: "TaiSanHuHais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuDungDichVus",
                table: "SuDungDichVus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Phongs",
                table: "Phongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiPhongs",
                table: "LoaiPhongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhachLuuTrus",
                table: "KhachLuuTrus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DichVus",
                table: "DichVus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatPhongs",
                table: "DatPhongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDatPhongs",
                table: "ChiTietDatPhongs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChinhSachGias",
                table: "ChinhSachGias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChinhSachGias_LoaiPhongs_LoaiPhongId",
                table: "ChinhSachGias",
                column: "LoaiPhongId",
                principalTable: "LoaiPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatPhongs_DatPhongs_DatPhongId",
                table: "ChiTietDatPhongs",
                column: "DatPhongId",
                principalTable: "DatPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatPhongs_Phongs_PhongId",
                table: "ChiTietDatPhongs",
                column: "PhongId",
                principalTable: "Phongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_KhachHangs_KhachHangId",
                table: "DatPhongs",
                column: "KhachHangId",
                principalTable: "KhachHangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_DatPhongs_DatPhongId",
                table: "HoaDons",
                column: "DatPhongId",
                principalTable: "DatPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons",
                column: "LeTanId",
                principalTable: "LeTans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachLuuTrus_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "KhachLuuTrus",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phongs_LoaiPhongs_LoaiPhongId",
                table: "Phongs",
                column: "LoaiPhongId",
                principalTable: "LoaiPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuDungDichVus_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "SuDungDichVus",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuDungDichVus_DichVus_DichVuId",
                table: "SuDungDichVus",
                column: "DichVuId",
                principalTable: "DichVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHais_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "TaiSanHuHais",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHais_ThietBiPhongs_ThietBiPhongId",
                table: "TaiSanHuHais",
                column: "ThietBiPhongId",
                principalTable: "ThietBiPhongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHais_TrucBuongs_TrucBuongId",
                table: "TaiSanHuHais",
                column: "TrucBuongId",
                principalTable: "TrucBuongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBiPhongs_Phongs_PhongId",
                table: "ThietBiPhongs",
                column: "PhongId",
                principalTable: "Phongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBiPhongs_ThietBis_ThietBiId",
                table: "ThietBiPhongs",
                column: "ThietBiId",
                principalTable: "ThietBis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChinhSachGias_LoaiPhongs_LoaiPhongId",
                table: "ChinhSachGias");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatPhongs_DatPhongs_DatPhongId",
                table: "ChiTietDatPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDatPhongs_Phongs_PhongId",
                table: "ChiTietDatPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_KhachHangs_KhachHangId",
                table: "DatPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DatPhongs_DatPhongId",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_LeTans_LeTanId",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachLuuTrus_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "KhachLuuTrus");

            migrationBuilder.DropForeignKey(
                name: "FK_Phongs_LoaiPhongs_LoaiPhongId",
                table: "Phongs");

            migrationBuilder.DropForeignKey(
                name: "FK_SuDungDichVus_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "SuDungDichVus");

            migrationBuilder.DropForeignKey(
                name: "FK_SuDungDichVus_DichVus_DichVuId",
                table: "SuDungDichVus");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHais_ChiTietDatPhongs_ChiTietDatPhongId",
                table: "TaiSanHuHais");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHais_ThietBiPhongs_ThietBiPhongId",
                table: "TaiSanHuHais");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiSanHuHais_TrucBuongs_TrucBuongId",
                table: "TaiSanHuHais");

            migrationBuilder.DropForeignKey(
                name: "FK_ThietBiPhongs_Phongs_PhongId",
                table: "ThietBiPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_ThietBiPhongs_ThietBis_ThietBiId",
                table: "ThietBiPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThietBis",
                table: "ThietBis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThietBiPhongs",
                table: "ThietBiPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaiSanHuHais",
                table: "TaiSanHuHais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuDungDichVus",
                table: "SuDungDichVus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Phongs",
                table: "Phongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiPhongs",
                table: "LoaiPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhachLuuTrus",
                table: "KhachLuuTrus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DichVus",
                table: "DichVus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatPhongs",
                table: "DatPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDatPhongs",
                table: "ChiTietDatPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChinhSachGias",
                table: "ChinhSachGias");

            migrationBuilder.RenameTable(
                name: "ThietBis",
                newName: "ThietBi");

            migrationBuilder.RenameTable(
                name: "ThietBiPhongs",
                newName: "ThietBiPhong");

            migrationBuilder.RenameTable(
                name: "TaiSanHuHais",
                newName: "TaiSanHuHai");

            migrationBuilder.RenameTable(
                name: "SuDungDichVus",
                newName: "SuDungDichVu");

            migrationBuilder.RenameTable(
                name: "Phongs",
                newName: "Phong");

            migrationBuilder.RenameTable(
                name: "LoaiPhongs",
                newName: "LoaiPhong");

            migrationBuilder.RenameTable(
                name: "KhachLuuTrus",
                newName: "KhachLuuTru");

            migrationBuilder.RenameTable(
                name: "HoaDons",
                newName: "HoaDon");

            migrationBuilder.RenameTable(
                name: "DichVus",
                newName: "DichVu");

            migrationBuilder.RenameTable(
                name: "DatPhongs",
                newName: "DatPhong");

            migrationBuilder.RenameTable(
                name: "ChiTietDatPhongs",
                newName: "ChiTietDatPhong");

            migrationBuilder.RenameTable(
                name: "ChinhSachGias",
                newName: "ChinhSachGia");

            migrationBuilder.RenameIndex(
                name: "IX_ThietBiPhongs_ThietBiId",
                table: "ThietBiPhong",
                newName: "IX_ThietBiPhong_ThietBiId");

            migrationBuilder.RenameIndex(
                name: "IX_ThietBiPhongs_PhongId",
                table: "ThietBiPhong",
                newName: "IX_ThietBiPhong_PhongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHais_TrucBuongId",
                table: "TaiSanHuHai",
                newName: "IX_TaiSanHuHai_TrucBuongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHais_ThietBiPhongId",
                table: "TaiSanHuHai",
                newName: "IX_TaiSanHuHai_ThietBiPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_TaiSanHuHais_ChiTietDatPhongId",
                table: "TaiSanHuHai",
                newName: "IX_TaiSanHuHai_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_SuDungDichVus_DichVuId",
                table: "SuDungDichVu",
                newName: "IX_SuDungDichVu_DichVuId");

            migrationBuilder.RenameIndex(
                name: "IX_SuDungDichVus_ChiTietDatPhongId",
                table: "SuDungDichVu",
                newName: "IX_SuDungDichVu_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_Phongs_SoPhong",
                table: "Phong",
                newName: "IX_Phong_SoPhong");

            migrationBuilder.RenameIndex(
                name: "IX_Phongs_LoaiPhongId",
                table: "Phong",
                newName: "IX_Phong_LoaiPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_KhachLuuTrus_ChiTietDatPhongId",
                table: "KhachLuuTru",
                newName: "IX_KhachLuuTru_ChiTietDatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_LeTanId",
                table: "HoaDon",
                newName: "IX_HoaDon_LeTanId");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_DatPhongId",
                table: "HoaDon",
                newName: "IX_HoaDon_DatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhongs_KhachHangId",
                table: "DatPhong",
                newName: "IX_DatPhong_KhachHangId");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatPhongs_PhongId",
                table: "ChiTietDatPhong",
                newName: "IX_ChiTietDatPhong_PhongId");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDatPhongs_DatPhongId",
                table: "ChiTietDatPhong",
                newName: "IX_ChiTietDatPhong_DatPhongId");

            migrationBuilder.RenameIndex(
                name: "IX_ChinhSachGias_LoaiPhongId",
                table: "ChinhSachGia",
                newName: "IX_ChinhSachGia_LoaiPhongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThietBi",
                table: "ThietBi",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThietBiPhong",
                table: "ThietBiPhong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaiSanHuHai",
                table: "TaiSanHuHai",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuDungDichVu",
                table: "SuDungDichVu",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Phong",
                table: "Phong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiPhong",
                table: "LoaiPhong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhachLuuTru",
                table: "KhachLuuTru",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DichVu",
                table: "DichVu",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatPhong",
                table: "DatPhong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDatPhong",
                table: "ChiTietDatPhong",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChinhSachGia",
                table: "ChinhSachGia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChinhSachGia_LoaiPhong_LoaiPhongId",
                table: "ChinhSachGia",
                column: "LoaiPhongId",
                principalTable: "LoaiPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatPhong_DatPhong_DatPhongId",
                table: "ChiTietDatPhong",
                column: "DatPhongId",
                principalTable: "DatPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDatPhong_Phong_PhongId",
                table: "ChiTietDatPhong",
                column: "PhongId",
                principalTable: "Phong",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhong_KhachHangs_KhachHangId",
                table: "DatPhong",
                column: "KhachHangId",
                principalTable: "KhachHangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_DatPhong_DatPhongId",
                table: "HoaDon",
                column: "DatPhongId",
                principalTable: "DatPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_LeTans_LeTanId",
                table: "HoaDon",
                column: "LeTanId",
                principalTable: "LeTans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachLuuTru_ChiTietDatPhong_ChiTietDatPhongId",
                table: "KhachLuuTru",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phong_LoaiPhong_LoaiPhongId",
                table: "Phong",
                column: "LoaiPhongId",
                principalTable: "LoaiPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuDungDichVu_ChiTietDatPhong_ChiTietDatPhongId",
                table: "SuDungDichVu",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuDungDichVu_DichVu_DichVuId",
                table: "SuDungDichVu",
                column: "DichVuId",
                principalTable: "DichVu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHai_ChiTietDatPhong_ChiTietDatPhongId",
                table: "TaiSanHuHai",
                column: "ChiTietDatPhongId",
                principalTable: "ChiTietDatPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHai_ThietBiPhong_ThietBiPhongId",
                table: "TaiSanHuHai",
                column: "ThietBiPhongId",
                principalTable: "ThietBiPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiSanHuHai_TrucBuongs_TrucBuongId",
                table: "TaiSanHuHai",
                column: "TrucBuongId",
                principalTable: "TrucBuongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBiPhong_Phong_PhongId",
                table: "ThietBiPhong",
                column: "PhongId",
                principalTable: "Phong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBiPhong_ThietBi_ThietBiId",
                table: "ThietBiPhong",
                column: "ThietBiId",
                principalTable: "ThietBi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
