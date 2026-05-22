using BookingRoom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingRoom.Data.Configurations
{
    public class BusinessConfiguration
    {
        public class LoaiPhongConfig : IEntityTypeConfiguration<LoaiPhong>
        {
            public void Configure(EntityTypeBuilder<LoaiPhong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.GiaCoBan).HasColumnType("decimal(18,2)");
                builder.Property(x => x.TenLoaiPhong).HasMaxLength(100).IsRequired();
                builder.HasQueryFilter(x => !x.IsDeleted); // Tự động ẩn dữ liệu đã bị xóa mềm
            }
        }

        public class ChinhSachGiaConfig : IEntityTypeConfiguration<ChinhSachGia>
        {
            public void Configure(EntityTypeBuilder<ChinhSachGia> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.GiaTriDieuChinh).HasColumnType("decimal(18,2)");
                builder.HasOne(x => x.LoaiPhong).WithMany(x => x.ChinhSachGias).HasForeignKey(x => x.LoaiPhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class PhongConfig : IEntityTypeConfiguration<Phong>
        {
            public void Configure(EntityTypeBuilder<Phong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.SoPhong).HasMaxLength(20).IsRequired();
                builder.HasIndex(x => x.SoPhong).IsUnique(); // Ràng buộc Unique
                builder.HasOne(x => x.LoaiPhong).WithMany(x => x.Phongs).HasForeignKey(x => x.LoaiPhongId).OnDelete(DeleteBehavior.Restrict);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class DatPhongConfig : IEntityTypeConfiguration<DatPhong>
        {
            public void Configure(EntityTypeBuilder<DatPhong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.KhachHang).WithMany(x => x.DatPhongs).HasForeignKey(x => x.KhachHangId).OnDelete(DeleteBehavior.Restrict);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class ChiTietDatPhongConfig : IEntityTypeConfiguration<ChiTietDatPhong>
        {
            public void Configure(EntityTypeBuilder<ChiTietDatPhong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.GiaThucTe).HasColumnType("decimal(18,2)");
                builder.HasOne(x => x.DatPhong).WithMany(x => x.ChiTietDatPhongs).HasForeignKey(x => x.DatPhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(x => x.Phong).WithMany(x => x.ChiTietDatPhongs).HasForeignKey(x => x.PhongId).OnDelete(DeleteBehavior.SetNull);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class HoaDonConfig : IEntityTypeConfiguration<HoaDon>
        {
            public void Configure(EntityTypeBuilder<HoaDon> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.TongTienBooking).HasColumnType("decimal(18,2)");

                builder.HasOne(x => x.DatPhong)
                       .WithOne(x => x.HoaDon)
                       .HasForeignKey<HoaDon>(x => x.DatPhongId) 
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class ChiTietHoaDonConfig : IEntityTypeConfiguration<ChiTietHoaDon>
        {
            public void Configure(EntityTypeBuilder<ChiTietHoaDon> builder)
            {
                builder.HasKey(x => x.Id);

                //  Ràng buộc kiểu dữ liệu tiền tệ để không bị sai số
                builder.Property(x => x.SoTienPhaiTra).HasColumnType("decimal(18,2)");
                builder.Property(x => x.SoTienDaThanhToan).HasColumnType("decimal(18,2)");

                builder.HasOne(x => x.HoaDon)
                       .WithMany(h => h.ChiTietHoaDons)
                       .HasForeignKey(x => x.HoaDonId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.ChiTietDatPhong)
                       .WithMany()
                       .HasForeignKey(x => x.ChiTietDatPhongId)
                       .OnDelete(DeleteBehavior.Restrict);
               
                builder.HasOne(x => x.LeTan)
                       .WithMany()
                       .HasForeignKey(x => x.LeTanId)
                       .OnDelete(DeleteBehavior.SetNull);

                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class DichVuConfig : IEntityTypeConfiguration<DichVu>
        {
            public void Configure(EntityTypeBuilder<DichVu> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Gia).HasColumnType("decimal(18,2)");
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class SuDungDichVuConfig : IEntityTypeConfiguration<SuDungDichVu>
        {
            public void Configure(EntityTypeBuilder<SuDungDichVu> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.ThanhTien).HasColumnType("decimal(18,2)");
                builder.HasOne(x => x.ChiTietDatPhong).WithMany(x => x.SuDungDichVus).HasForeignKey(x => x.ChiTietDatPhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(x => x.DichVu).WithMany(x => x.SuDungDichVus).HasForeignKey(x => x.DichVuId).OnDelete(DeleteBehavior.Restrict);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

        public class TaiSanHuHaiConfig : IEntityTypeConfiguration<TaiSanHuHai>
        {
            public void Configure(EntityTypeBuilder<TaiSanHuHai> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.PhiDenBu).HasColumnType("decimal(18,2)");
                builder.HasOne(x => x.ChiTietDatPhong).WithMany(x => x.TaiSanHuHais).HasForeignKey(x => x.ChiTietDatPhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(x => x.ThietBiPhong).WithMany(x => x.TaiSanHuHais).HasForeignKey(x => x.ThietBiPhongId).OnDelete(DeleteBehavior.Restrict);
                builder.HasOne(x => x.TrucBuong)
                    .WithMany(x => x.TaiSanHuHais)
                    .HasForeignKey(x => x.TrucBuongId)
                    .OnDelete(DeleteBehavior.SetNull);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class KhachLuuTruConfig : IEntityTypeConfiguration<KhachLuuTru>
        {
            public void Configure(EntityTypeBuilder<KhachLuuTru> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.ChiTietDatPhong).WithMany(x => x.KhachLuuTrus).HasForeignKey(x => x.ChiTietDatPhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class ThietBiConfig : IEntityTypeConfiguration<ThietBi>
        {
            public void Configure(EntityTypeBuilder<ThietBi> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class ThietBiPhongConfig : IEntityTypeConfiguration<ThietBiPhong>
        {
            public void Configure(EntityTypeBuilder<ThietBiPhong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.Phong).WithMany(x => x.ThietBiPhongs).HasForeignKey(x => x.PhongId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(x => x.ThietBi).WithMany(x => x.ThietBiPhongs).HasForeignKey(x => x.ThietBiId).OnDelete(DeleteBehavior.Restrict);

                // THÊM DÒNG NÀY ĐỂ ĐỒNG BỘ
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }

    }
}