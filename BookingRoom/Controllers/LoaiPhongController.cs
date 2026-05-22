using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/typeroom")]
    [ApiController]
    public class LoaiPhongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoaiPhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLoaiPhongs()
        {
            try
            {
                var loaiPhongs = await _context.LoaiPhongs
                    .Select(lp => new LoaiPhongResponseVM
    {
                        Id = lp.Id, // Đổi MaLoaiPhong thành Id
                        TenLoaiPhong = lp.TenLoaiPhong,
                        GiaCoBan = lp.GiaCoBan,
                        SoNguoiToiDa = lp.SoNguoiToiDa,
                        AnhDaiDien = lp.AnhDaiDien,
                        MoTa = lp.MoTa
                    }).ToListAsync();

                return Ok(loaiPhongs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách loại phòng.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.RoomType.Create)]
        public async Task<IActionResult> CreateLoaiPhong([FromBody] LoaiPhongVM request)
        {
            try
            {
                var loaiPhong = new LoaiPhong
                {
                    TenLoaiPhong = request.TenLoaiPhong,
                    GiaCoBan = request.GiaCoBan,
                    SoNguoiToiDa = request.SoNguoiToiDa,
                    AnhDaiDien = request.AnhDaiDien,
                    MoTa = request.MoTa
                };

                _context.LoaiPhongs.Add(loaiPhong);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Thêm loại phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm loại phòng.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AppPermissions.RoomType.Edit)]
        public async Task<IActionResult> UpdateLoaiPhong(Guid id, [FromBody] LoaiPhongVM request) // Dùng Guid
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
                if (loaiPhong == null)
                    return NotFound(new { Message = "Không tìm thấy loại phòng này." });

                loaiPhong.TenLoaiPhong = request.TenLoaiPhong;
                loaiPhong.GiaCoBan = request.GiaCoBan;
                loaiPhong.SoNguoiToiDa = request.SoNguoiToiDa;
                loaiPhong.AnhDaiDien = request.AnhDaiDien;
                loaiPhong.MoTa = request.MoTa;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật loại phòng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật loại phòng.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPermissions.RoomType.Delete)]
        public async Task<IActionResult> DeleteLoaiPhong(Guid id)
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
                if (loaiPhong == null)
                    return NotFound(new { Message = "Không tìm thấy loại phòng này." });

                // Logic cực kỳ chuẩn của bạn: Phải check xem có phòng nào đang dùng Loại này không
                // Đổi p.MaLoaiPhong thành p.LoaiPhongId cho đúng chuẩn khoá ngoại mới
                var dangSuDung = await _context.Phongs.AnyAsync(p => p.LoaiPhongId == id);

                if (dangSuDung)
                    return BadRequest(new { Message = "Không thể xóa vì đang có phòng thuộc loại này. Vui lòng xóa các phòng đó trước." });

                // Thay thế DaXoa = true bằng hàm Remove (sẽ được DbContext tự động convert sang Soft Delete)
                _context.LoaiPhongs.Remove(loaiPhong);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Đã xóa loại phòng!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi xóa loại phòng.", Details = ex.Message });
            }
        }
    }
}