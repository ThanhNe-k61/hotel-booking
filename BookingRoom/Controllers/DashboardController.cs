using BookingRoom.Constants;
using BookingRoom.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AppPermissions.Booking.View)]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("gantt")]
        public async Task<IActionResult> GetGanttData(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? tang = null) // Thêm thuộc tính tầng tùy chọn
        {
            try
            {
                // tạo querry lấy phòng
                var roomQuery = _context.Phongs
                    .AsNoTracking()
                    .Include(p => p.LoaiPhong)
                    .AsQueryable();

                // Lọc theo tầng nếu người dùng có chọn
                if (tang.HasValue)
                {
                    roomQuery = roomQuery.Where(p => p.Tang == tang.Value);
                }

                // Thực thi truy vấn Phòng
                var rooms = await roomQuery
                    .OrderBy(p => p.SoPhong)
                    .Select(p => new
                    {
                        id = p.Id,
                        soPhong = p.SoPhong,
                        tang = p.Tang,
                        loai = p.LoaiPhong != null ? p.LoaiPhong.TenLoaiPhong : "Chưa xếp loại"
                    })
                    .ToListAsync();

                // Lấy ra danh sách các ID Phòng hợp lệ sau khi lọc để áp dụng cho Booking
                var roomIds = rooms.Select(r => r.id).ToList();

                //  LẤY dữ liệu booking (Chỉ lấy Booking của các phòng trong danh sách roomIds)
                var bookings = await _context.ChiTietDatPhongs
                    .AsNoTracking()
                    .Where(c => c.PhongId != null &&
                                roomIds.Contains(c.PhongId.Value) && // Chìa khóa: Chỉ quét các phòng thuộc Tầng đã chọn
                                c.DatPhong.NgayNhanPhongDuKien <= endDate &&
                                c.DatPhong.NgayTraPhongDuKien >= startDate)
                    .Select(c => new
                    {
                        id = c.Id, // Mã Chi tiết đặt phòng
                        roomId = c.PhongId, // Mã Phòng (Map với trục Y của Gantt)
                        datPhongId = c.DatPhongId, // Mã Đặt phòng tổng

                        // Lấy thẳng thông tin Khách Hàng (Người đặt) thay vì quét Khách Lưu Trú
                        customerName = c.DatPhong.KhachHang != null ? c.DatPhong.KhachHang.HoTen : "Khách ẩn danh",
                        customerPhone = c.DatPhong.KhachHang != null ? c.DatPhong.KhachHang.SoDienThoai : "Không có thông tin",
                        customerCccdpassport = c.DatPhong.KhachHang != null ? c.DatPhong.KhachHang.CccdPassport : "Không có thông tin",

                        // Thời gian vẽ thanh Gantt
                        checkIn = c.ThoiGianNhanPhongThucTe ?? c.DatPhong.NgayNhanPhongDuKien,
                        checkOut = c.ThoiGianTraPhongThucTe ?? c.DatPhong.NgayTraPhongDuKien,

                        status = c.DatPhong.TrangThai,
                        price = c.GiaThucTe
                    })
                    .ToListAsync();

                return Ok(new { rooms, bookings });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi trong quá trình lấy dữ liệu biểu đồ Gantt",
                    Error = ex.Message
                });
            }
        }
    }
}