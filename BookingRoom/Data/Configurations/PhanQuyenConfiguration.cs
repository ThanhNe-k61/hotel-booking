using BookingRoom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingRoom.Data.Configuration
{
    public class PhanQuyenConfiguration : IEntityTypeConfiguration<VaiTroQuyen>, IEntityTypeConfiguration<NguoiDungVaiTro>
    {
        public void Configure(EntityTypeBuilder<VaiTroQuyen> builder)
        {
            builder.HasKey(vq => new { vq.VaiTroId, vq.QuyenId });
            builder.HasOne(vq => vq.VaiTro).WithMany(v => v.VaiTroQuyens).HasForeignKey(vq => vq.VaiTroId);
            builder.HasOne(vq => vq.Quyen).WithMany(q => q.VaiTroQuyens).HasForeignKey(vq => vq.QuyenId);
        }

        public void Configure(EntityTypeBuilder<NguoiDungVaiTro> builder)
        {
            builder.HasKey(nv => new { nv.NguoiDungId, nv.VaiTroId });
            builder.HasOne(nv => nv.NguoiDung).WithMany(n => n.NguoiDungVaiTros).HasForeignKey(nv => nv.NguoiDungId);
            builder.HasOne(nv => nv.VaiTro).WithMany(v => v.NguoiDungVaiTros).HasForeignKey(nv => nv.VaiTroId);
        }
    }
}