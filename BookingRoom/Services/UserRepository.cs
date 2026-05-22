using BookingRoom.Data;
using BookingRoom.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // xem danh sách người dùng với phân trang và tìm kiếm
        public async Task<PagedResultVM<object>> GetUsersAsync(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            // Chỉ lấy người chưa bị xóa
            var query = _context.NguoiDungs.Where(u => !u.IsDeleted).AsQueryable();

            // Lọc theo từ khóa (Tìm theo Tên hoặc Email)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(u => u.HoTen.Contains(keyword) || u.Email.Contains(keyword));
            }

            // Đếm tổng số lượng (trước khi cắt trang)
            int totalCount = await query.CountAsync();

            // Cắt trang (Pagination)
            var items = await query
                .OrderByDescending(u => u.Id) // Sắp xếp người mới nhất lên đầu
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.TenDangNhap,
                    u.Email,
                    u.HoTen,
                    u.SoDienThoai,
                    u.TrangThai,
                    u.DefaultRole,
                    VaiTros = u.NguoiDungVaiTros.Select(nv => nv.VaiTro.TenVaiTro).ToList()
                })
                .ToListAsync();

            return new PagedResultVM<object>
            {
                Items = items.Cast<object>().ToList(),
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        // xoá mềm tài khoản người dùng
        public async Task<bool> SoftDeleteUserAsync(Guid userId)
        {
            var user = await _context.NguoiDungs.FindAsync(userId);
            if (user == null || user.IsDeleted) return false;

            // Đánh dấu xóa và khóa tài khoản
            user.IsDeleted = true;
            user.TrangThai = "Đã khóa";

            _context.NguoiDungs.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}