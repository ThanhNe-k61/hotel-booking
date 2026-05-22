using BookingRoom.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookingRoom.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ChucNang> ChucNangs { get; set; }
        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<VaiTroQuyen> VaiTroQuyens { get; set; }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<NguoiDungVaiTro> NguoiDungVaiTros { get; set; }
        public DbSet<LeTan> LeTans { get; set; }
        public DbSet<TrucBuong> TrucBuongs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<ChinhSachGia> ChinhSachGias { get; set; }
        public DbSet<ChiTietDatPhong> ChiTietDatPhongs { get; set; }
        public DbSet<DatPhong> DatPhongs { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhachLuuTru> KhachLuuTrus { get; set; }
        
        public DbSet<SuDungDichVu> SuDungDichVus { get; set; }
        public DbSet<TaiSanHuHai> TaiSanHuHais { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<ThietBiPhong> ThietBiPhongs { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply toàn bộ file Configuration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Duyệt qua tất cả các entity đang được theo dõi có thay đổi
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        // Không cho phép sửa CreatedAt
                        entry.Property(x => x.CreatedAt).IsModified = false;
                        break;

                    case EntityState.Deleted: // Biến thao tác Xóa cứng (Hard Delete) thành Xóa mềm (Soft Delete)
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}