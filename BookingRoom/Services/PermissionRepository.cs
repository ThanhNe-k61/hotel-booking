using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Services
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        public PermissionRepository(ApplicationDbContext context) => _context = context;

        // tạo vai trò
        public async Task<bool> TaoVaiTroMoiAsync(CreateRoleVM model)
        {
            try
            {
                // Tránh tạo trùng tên
                if (await _context.VaiTros.AnyAsync(r => r.TenVaiTro == model.TenVaiTro))
                    return false;

                _context.VaiTros.Add(new VaiTro { TenVaiTro = model.TenVaiTro });
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo vai trò mới: " + ex.Message);
            }
        }

        // lấy data vẽ checkbox ui
        public async Task<List<ChucNangTreeVM>> LayDanhSachQuyenDeVeUIAsync()
        {
            try
            {
                return await _context.ChucNangs.Include(c => c.Quyens)
                                     .Select(c => new ChucNangTreeVM
                                     {
                                        Id = c.Id,
                                        TenChucNang = c.TenChucNang,
                                        Quyens = c.Quyens.Select(q => new QuyenItemVM
                                                {
                                                        Id = q.Id,
                                                        TenQuyen = q.TenQuyen,
                                                        GiaTriQuyen = q.GiaTriQuyen
                                                }).ToList()
                                     }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách quyền: " + ex.Message);
            }
        }

        // Gắn quyền vào vai trò
        public async Task<bool> GanQuyenChoVaiTroAsync(AssignRolePermissionVM model)
        {
            try
            {
                var vaiTro = await _context.VaiTros.FindAsync(model.VaiTroId);
                if (vaiTro == null) return false;

                // Xóa sạch quyền cũ của Vai trò này
                var quyenCu = await _context.VaiTroQuyens.Where(vq => vq.VaiTroId == model.VaiTroId).ToListAsync();
                _context.VaiTroQuyens.RemoveRange(quyenCu);

                // Chèn danh sách quyền mới (từ các ô checkbox sếp vừa tick)
                foreach (var quyenId in model.QuyenIds)
                {
                    _context.VaiTroQuyens.Add(new VaiTroQuyen { VaiTroId = model.VaiTroId, QuyenId = quyenId });
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi gắn quyền cho vai trò: " + ex.Message);
            }
        }

        // Gắn vai trò cho nhân viên
        public async Task<bool> GanVaiTroChoNhanVienAsync(AssignUserRoleVM model)
        {
            try
            {
                var user = await _context.NguoiDungs.FindAsync(model.NguoiDungId);
                if (user == null) return false;

                // Xóa sạch vai trò cũ của nhân viên này
                var vaiTroCu = await _context.NguoiDungVaiTros.Where(nv => nv.NguoiDungId == model.NguoiDungId).ToListAsync();
                _context.NguoiDungVaiTros.RemoveRange(vaiTroCu);

                // Chèn danh sách vai trò mới
                foreach (var vaiTroId in model.VaiTroIds)
                {
                    _context.NguoiDungVaiTros.Add(new NguoiDungVaiTro { NguoiDungId = model.NguoiDungId, VaiTroId = vaiTroId });
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi gắn vai trò cho nhân viên: " + ex.Message);
            }
        }
        //  Lấy danh sách ID các Vai trò mà Nhân viên đang giữ (Phục vụ hiển thị Checkbox/Select đã chọn)
        public async Task<List<Guid>> LayDanhSachVaiTroCuaNhanVienAsync(Guid userId)
        {
            return await _context.NguoiDungVaiTros
                            .AsNoTracking()
                            .Where(nv => nv.NguoiDungId == userId && nv.VaiTroId != null)
                            .Select(nv => nv.VaiTroId.Value) 
                            .ToListAsync();
        }

        //  Lấy danh sách tất cả các Role (Phục vụ dropdown chọn Role)
        public async Task<object> LayDanhSachVaiTroAsync()
        {
            return await _context.VaiTros
                            .AsNoTracking()
                            .Select(v => new
                            {
                                v.Id,
                                v.TenVaiTro
                            })
                            .ToListAsync();
        }

        // Lấy danh sách ID các Quyền mà Role đang có (Phục vụ hiển thị Checkbox đã tick trên cây Phân quyền)
        public async Task<List<Guid>> LayDanhSachQuyenCuaVaiTroAsync(Guid roleId)
        {
            return await _context.VaiTroQuyens
                .AsNoTracking()
                .Where(vq => vq.VaiTroId == roleId && vq.QuyenId != null)
                .Select(vq => vq.QuyenId) 
                .ToListAsync();
        }
    }
}