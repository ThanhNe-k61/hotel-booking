using BookingRoom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingRoom.Data.Configuration
{
    public class NguoiDungConfigurations
    {
        public class NguoiDungConfig : IEntityTypeConfiguration<NguoiDung>
        {
            public void Configure(EntityTypeBuilder<NguoiDung> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => x.TenDangNhap).IsUnique();
                builder.HasIndex(x => x.Email).IsUnique();
                builder.Property(x => x.TenDangNhap).HasMaxLength(50).IsRequired();
                builder.Property(x => x.MatKhauHash).HasMaxLength(255).IsRequired();
                builder.Property(x => x.HoTen).HasMaxLength(100).IsRequired();
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class LeTanConfig : IEntityTypeConfiguration<LeTan>
        {
            public void Configure(EntityTypeBuilder<LeTan> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.MucLuongCoBan).HasColumnType("decimal(18,2)");
                builder.HasOne(x => x.NguoiDung)
                       .WithOne(u=>u.LeTan)
                       .HasForeignKey<LeTan>(x => x.NguoiDungId)//chỉ rõ rằng khóa ngoại là NguoiDungId trong bảng LeTan
                       .OnDelete(DeleteBehavior.Cascade); 
                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class TrucBuongConfig : IEntityTypeConfiguration<TrucBuong>
        {
            public void Configure(EntityTypeBuilder<TrucBuong> builder)
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.MucLuongCoBan).HasColumnType("decimal(18,2)");

                builder.HasOne(x => x.NguoiDung)
                       .WithOne(u=> u.TrucBuong)
                       .HasForeignKey<TrucBuong>(x => x.NguoiDungId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class KhachHangConfig : IEntityTypeConfiguration<KhachHang>
        {
            public void Configure(EntityTypeBuilder<KhachHang> builder)
            {
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.NguoiDung)
                       .WithOne(u=>u.KhachHang)
                       .HasForeignKey<KhachHang>(x => x.NguoiDungId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasQueryFilter(x => !x.IsDeleted);
            }
        }
        public class NguoiDungVaiTroConfig : IEntityTypeConfiguration<NguoiDungVaiTro>
        {
            public void Configure(EntityTypeBuilder<NguoiDungVaiTro> builder)
            {
                // Khai báo Khóa chính kép
                builder.HasKey(x => new { x.NguoiDungId, x.VaiTroId });
                builder.HasOne(x => x.NguoiDung)
                       .WithMany(u => u.NguoiDungVaiTros)
                       .HasForeignKey(x => x.NguoiDungId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(x => x.VaiTro)
                       .WithMany(v => v.NguoiDungVaiTros)
                       .HasForeignKey(x => x.VaiTroId)
                       .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}