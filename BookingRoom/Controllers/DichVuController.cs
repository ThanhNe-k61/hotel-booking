using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/services")]
    [ApiController]
    // KHÔNG dùng Authorize chung ở đây nữa, phân quyền chi tiết cho từng hàm bên dưới
    public class DichVuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDichVus()
        {
            try
            {
                var dichVus = await _context.DichVus
                    .Select(dv => new
                    {
                        dv.Id, // Đã đổi sang Guid (từ BaseEntity)
                        dv.TenDichVu,
                        dv.Gia,
                        dv.MoTa,
                        dv.LoaiDichVu,
                        dv.TrangThai
                    })
                    .ToListAsync();

                return Ok(dichVus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách dịch vụ.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Service.Create)]
        public async Task<IActionResult> CreateDichVu([FromBody] DichVuVM request)
        {
            try
            {
                var dichVu = new DichVu
                {
                    TenDichVu = request.TenDichVu,
                    Gia = request.Gia,
                    MoTa = request.MoTa,
                    LoaiDichVu = request.LoaiDichVu,
                    TrangThai = request.TrangThai
                };

                _context.DichVus.Add(dichVu);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Thêm dịch vụ thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi thêm dịch vụ.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AppPermissions.Service.Edit)]
        public async Task<IActionResult> UpdateDichVu(Guid id, [FromBody] DichVuVM request) 
        {
            try 
            {
                var dichVu = await _context.DichVus.FindAsync(id);

                if (dichVu == null)
                    return NotFound(new { Message = "Không tìm thấy dịch vụ." });

                dichVu.TenDichVu = request.TenDichVu;
                dichVu.Gia = request.Gia;
                dichVu.MoTa = request.MoTa;
                dichVu.LoaiDichVu = request.LoaiDichVu;
                dichVu.TrangThai = request.TrangThai;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Cập nhật dịch vụ thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật dịch vụ.", Details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = AppPermissions.Service.Delete)]
        public async Task<IActionResult> DeleteDichVu(Guid id) 
        {
            try
            {
                var dichVu = await _context.DichVus.FindAsync(id);

                if (dichVu == null)
                    return NotFound(new { Message = "Không tìm thấy dịch vụ." });

                // Tận dụng OnModelCreating: Gọi Remove() nhưng sẽ biến thành Update (IsDeleted = true)
                _context.DichVus.Remove(dichVu);

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã xóa dịch vụ!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi xóa dịch vụ.", Details = ex.Message });
            }
        }
    }
}