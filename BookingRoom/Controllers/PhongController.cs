using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = AppPermissions.Room.View)] 
        public async Task<IActionResult> GetPhongs()
        {
            try
            {
                var phongs = await _context.Phongs
                    .Include(p => p.LoaiPhong) // Kéo theo thông tin Loại Phòng
                    .Select(p => new PhongResponseVM
                    {
                        Id = p.Id,
                        LoaiPhongId = p.LoaiPhongId,
                        TenLoaiPhong = p.LoaiPhong != null ? p.LoaiPhong.TenLoaiPhong : "Không xác định",
                        SoPhong = p.SoPhong,
                        Tang = p.Tang,
                        TrangThai = p.TrangThai
                    })
                    .OrderBy(p => p.SoPhong)
                    .ToListAsync();

                return Ok(phongs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã có lỗi xảy ra khi lấy danh sách phòng.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Room.Create)]
        public async Task<IActionResult> CreatePhong([FromBody] PhongVM request)
        {
            try
            {
                // Kiểm tra xem Số phòng đã tồn tại chưa (vì SoPhong là Unique)
                var exists = await _context.Phongs.AnyAsync(p => p.SoPhong == request.SoPhong);
                if (exists)
                    return BadRequest(new { Message = "Số phòng này đã tồn tại trong hệ thống." });

                var phong = new Phong
                {
                    LoaiPhongId = request.LoaiPhongId,
                    SoPhong = request.SoPhong,
                    Tang = request.Tang,
                    TrangThai = request.TrangThai
                };

                _context.Phongs.Add(phong);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Thêm phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã có lỗi xảy ra khi thêm phòng.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AppPermissions.Room.Edit)]
        public async Task<IActionResult> UpdatePhong(Guid id, [FromBody] PhongVM request)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(id);
                if (phong == null)
                    return NotFound(new { Message = "Không tìm thấy phòng này." });

                // Kiểm tra trùng Số phòng (nếu họ đổi sang số phòng của phòng khác)
                var exists = await _context.Phongs.AnyAsync(p => p.SoPhong == request.SoPhong && p.Id != id);
                if (exists)
                    return BadRequest(new { Message = "Số phòng này đã được sử dụng." });

                phong.LoaiPhongId = request.LoaiPhongId;
                phong.SoPhong = request.SoPhong;
                phong.Tang = request.Tang;
                phong.TrangThai = request.TrangThai;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã có lỗi xảy ra khi cập nhật phòng.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPermissions.Room.Delete)]
        public async Task<IActionResult> DeletePhong(Guid id)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(id);
                if (phong == null)
                    return NotFound(new { Message = "Không tìm thấy phòng này." });

                // Ràng buộc: Nếu phòng đang có khách đặt/ở thì KHÔNG được xóa
                var dangHoatDong = await _context.ChiTietDatPhongs
                    .AnyAsync(c => c.PhongId == id &&
                                  (c.DatPhong.TrangThai == "Đã nhận phòng" || c.DatPhong.TrangThai == "Chờ nhận phòng"));

                if (dangHoatDong)
                    return BadRequest(new { Message = "Không thể xóa vì phòng này đang có khách đặt hoặc đang ở." });

                _context.Phongs.Remove(phong);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Đã xóa phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã có lỗi xảy ra khi xóa phòng.", Details = ex.Message });
            }
        }
    }
}