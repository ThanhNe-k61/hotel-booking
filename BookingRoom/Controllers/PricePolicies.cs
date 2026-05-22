using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/pricing-policies")] 
    [ApiController]
    public class ChinhSachGiaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPricingService _pricingService;

        public ChinhSachGiaController(ApplicationDbContext context, IPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        // get all
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetChinhSachGias()
        {
            try
            {
                var policies = await _context.ChinhSachGias
                    .Include(c => c.LoaiPhong) // Kéo theo dữ liệu Loại Phòng
                    .OrderByDescending(c => c.DoUuTien)
                    .Select(c => new
                    {
                        c.Id,
                        c.LoaiPhongId,
                        TenLoaiPhong = c.LoaiPhong != null ? c.LoaiPhong.TenLoaiPhong : "Tất cả",
                        c.TenChinhSach,
                        c.TuNgay,
                        c.DenNgay,
                        c.ApDungChoThu,
                        c.LoaiDieuChinh,
                        c.GiaTriDieuChinh,
                        c.DoUuTien,
                        c.TrangThai
                    })
                    .ToListAsync();

                return Ok(policies);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi lấy danh sách chính sách giá.", Details = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> CreateChinhSach([FromBody] ChinhSachGiaVM request)
        {
            try
            {
                if (request.TuNgay.HasValue && request.DenNgay.HasValue && request.TuNgay > request.DenNgay)
                    return BadRequest(new { Message = "Từ ngày không được lớn hơn Đến ngày." });

                // Kiểm tra xem Loại Phòng có tồn tại không
                var loaiPhongExists = await _context.LoaiPhongs.AnyAsync(lp => lp.Id == request.LoaiPhongId);
                if (!loaiPhongExists)
                    return BadRequest(new { Message = "Loại phòng không tồn tại." });

                var cs = new ChinhSachGia
                {
                    LoaiPhongId = request.LoaiPhongId,
                    TenChinhSach = request.TenChinhSach,
                    TuNgay = request.TuNgay,
                    DenNgay = request.DenNgay,
                    ApDungChoThu = request.ApDungChoThu,
                    LoaiDieuChinh = request.LoaiDieuChinh,
                    GiaTriDieuChinh = request.GiaTriDieuChinh,
                    DoUuTien = request.DoUuTien,
                    TrangThai = request.TrangThai
                };

                _context.ChinhSachGias.Add(cs);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Thêm chính sách thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi tạo chính sách giá.", Details = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> UpdateChinhSach(Guid id, [FromBody] ChinhSachGiaVM request) // Dùng Guid
        {
            try
            {
                if (request.TuNgay.HasValue && request.DenNgay.HasValue && request.TuNgay > request.DenNgay)
                    return BadRequest(new { Message = "Từ ngày không được lớn hơn Đến ngày." });

                var cs = await _context.ChinhSachGias.FindAsync(id);
                if (cs == null) return NotFound(new { Message = "Không tìm thấy chính sách." });

                // Cập nhật thông tin
                cs.LoaiPhongId = request.LoaiPhongId;
                cs.TenChinhSach = request.TenChinhSach;
                cs.TuNgay = request.TuNgay;
                cs.DenNgay = request.DenNgay;
                cs.ApDungChoThu = request.ApDungChoThu;
                cs.LoaiDieuChinh = request.LoaiDieuChinh;
                cs.GiaTriDieuChinh = request.GiaTriDieuChinh;
                cs.DoUuTien = request.DoUuTien;
                cs.TrangThai = request.TrangThai;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi cập nhật chính sách giá.", Details = ex.Message });
            }
        }
        // Toggle sẽ lật giữa "Kích hoạt" và "Tạm ngưng"
        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> ToggleChinhSach(Guid id) // Dùng Guid
        {
            try
            {
                var cs = await _context.ChinhSachGias.FindAsync(id);
                if (cs == null) return NotFound(new { Message = "Không tìm thấy chính sách." });
                // Lật trạng thái
                cs.TrangThai = cs.TrangThai == "Kích hoạt" ? "Tạm ngưng" : "Kích hoạt";
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã thay đổi trạng thái!", TrangThai = cs.TrangThai });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi thay đổi trạng thái chính sách giá.", Details = ex.Message });
            }
        }
 
        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        public async Task<IActionResult> DeleteChinhSach(Guid id) 
        {
            try
            {
                var cs = await _context.ChinhSachGias.FindAsync(id);
                if (cs == null) return NotFound(new { Message = "Không tìm thấy chính sách." });

                // Xóa mềm: Bật cờ IsDeleted thay vì gọi _context.Remove()
                cs.IsDeleted = true;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã xóa chính sách!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi xóa chính sách giá.", Details = ex.Message });
            }
        }
        [HttpGet("list-price")]
        [AllowAnonymous]
        public async Task<IActionResult> CalculatePrice([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] Guid? loaiPhongId = null)
        {
            try
            {
                if (startDate >= endDate)
                    return BadRequest(new { Message = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc." });

                try
                {
                    // Chỉ cần gọi Service ra làm việc, Controller không cần quan tâm chi tiết!
                    var result = await _pricingService.GetPriceMatrixAsync(startDate, endDate, loaiPhongId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Lỗi hệ thống khi xem ma trận giá.", Detail = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi tính toán giá.", Details = ex.Message });
            }
        }
    }
}