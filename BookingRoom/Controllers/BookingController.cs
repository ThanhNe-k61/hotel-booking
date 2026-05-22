using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly ApplicationDbContext _context;

        public BookingController(IBookingRepository bookingRepo, ApplicationDbContext context)
        {
            _bookingRepo = bookingRepo;
            _context = context;
        }

        // Khách hàng tìm phòng trống
        [HttpGet("available-rooms")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate.Date >= endDate.Date)
                    return BadRequest(new { Message = "Ngày không hợp lệ." });

                var result = await _bookingRepo.GetAvailableRoomsAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi tìm phòng trống.",
                    Details = ex.Message
                });
            }
        }

        // Tạo Booking
        [HttpPost]
        //[Authorize(Policy = AppPermissions.Booking.Create)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingVM request)
        {
            try
            {
                var bookingId = await _bookingRepo.CreateBookingAsync(request);
                return Ok(new { Message = "Đã ghi nhận đặt phòng thành công!", BookingId = bookingId });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi tạo booking.",
                    Details = ex.Message
                });
            }
        }
        // Lễ tân Check-in
        [HttpPost("{id}/check-in")]
        //[Authorize(Policy = AppPermissions.Reception.CheckIn)]
        [AllowAnonymous]
        public async Task<IActionResult> CheckIn(Guid id, [FromBody] CheckInVM request)
        {
            try
            {
                await _bookingRepo.CheckInAsync(id, request);
                return Ok(new { Message = "Đã check-in thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi check-in.",
                    Details = ex.Message
                });
            }
        }

        // BƯỚC 1: Lễ tân yêu cầu kiểm phòng
        [HttpPost("rooms/{phongId}/request-inspection")]
        //[Authorize(Policy = AppPermissions.Reception.CheckOut)]
        [AllowAnonymous]
        public async Task<IActionResult> RequestInspection(Guid phongId)
        {
            try
            {
                await _bookingRepo.RequestCheckoutInspectionAsync(phongId);
                return Ok(new { Message = "Đã gửi yêu cầu kiểm tra cho Trực buồng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi gửi yêu cầu kiểm tra.",
                    Details = ex.Message
                });
            }
        }

        // BƯỚC 2: Trực buồng nhập đồ dùng/hư hại và Hoàn tất
        [HttpPost("housekeeping-inspection")]
        //[Authorize(Policy = AppPermissions.Housekeeping.ReportDamage)]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitInspection([FromBody] HousekeepingInspectVM request)
        {
            try
            {
                // Giả lập lấy ID Trực buồng từ Token JWT
                Guid trucBuongId = Guid.NewGuid();

                await _bookingRepo.SubmitHousekeepingInspectionAsync(request, trucBuongId);
                return Ok(new { Message = "Kiểm phòng hoàn tất. Lễ tân có thể thanh toán!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi hoàn tất kiểm phòng.",
                    Details = ex.Message
                });
            }
        }

        // BƯỚC 2.5: Lễ tân lấy Folio đọc cho khách
        [HttpGet("{id}/invoice")]
        //[Authorize(Policy = AppPermissions.Bill.View)]
        [AllowAnonymous]
        public async Task<IActionResult> GetInvoice(Guid id)
        {
            try
            {
                var invoice = await _bookingRepo.GetInvoiceDetailsAsync(id);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi lấy thông tin hóa đơn.",
                    Details = ex.Message
                });
            }
        }
        // BƯỚC 3A: Gọi thêm dịch vụ lên phòng (Room Service, Spa...)
        [HttpPost("add-service")]
        //[Authorize(Policy = AppPermissions.Service.Create)]
        [AllowAnonymous]
        public async Task<IActionResult> AddServiceToRoom([FromBody] AddServiceUsageVM request)
        {
            try
            {
                await _bookingRepo.AddServiceToRoomAsync(request);
                return Ok(new { Message = "Đã thêm dịch vụ thành công. Công nợ hóa đơn đã được cập nhật!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi thêm dịch vụ.",
                    Details = ex.Message
                });
            }
        }

        // BƯỚC 3B: Tách bill
        [HttpPost("split-invoice")]
        //[Authorize(Policy = AppPermissions.Bill.Create)]
        [AllowAnonymous]
        public async Task<IActionResult> SplitInvoice([FromBody] SplitBillByRoomVM request)
        {
            try
            {
                var newInvoiceId = await _bookingRepo.SplitInvoiceByRoomAsync(request);
                return Ok(new { Message = "Đã tách hóa đơn thành công!", NewHoaDonId = newInvoiceId });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi tách hóa đơn.",
                    Details = ex.Message
                });
            }
        }

        // BƯỚC 4: Chốt Checkout
        // "{id}" -> "{datPhongId}"
        [HttpPost("{datPhongId}/confirm-checkout")]
        [AllowAnonymous]
        //[Authorize(Policy = AppPermissions.Reception.CheckOut)]
        // lấy leTanId từ [FromQuery] luôn nhé, hoặc lấy từ Token
        public async Task<IActionResult> ConfirmCheckout(Guid datPhongId, [FromQuery] string phuongThucTT, [FromQuery] Guid leTanId)
        {
            try
            {
                await _bookingRepo.ConfirmPaymentAndCheckoutAsync(datPhongId, phuongThucTT, leTanId);
                return Ok(new { Message = "Thanh toán thành công. Khách đã trả phòng!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    Message = "Đã xảy ra lỗi khi xác nhận thanh toán.",
                    Details = ex.Message
                });
            }
        }
        //[HttpPost("heal-corrupted-prices")]
        //[AllowAnonymous]
        //public async Task<IActionResult> HealCorruptedPrices([FromServices] IPricingService pricingService)
        //{
        //    var datPhongs = await _context.DatPhongs
        //        .Include(d => d.ChiTietDatPhongs).ThenInclude(c => c.Phong).ThenInclude(p => p.LoaiPhong)
        //        .Include(d => d.HoaDon).ThenInclude(h => h.ChiTietHoaDons)
        //        .ToListAsync();

        //    int fixedCount = 0;

        //    foreach (var dp in datPhongs)
        //    {
        //        decimal newTotal = 0;

        //        foreach (var ct in dp.ChiTietDatPhongs)
        //        {
        //            if (ct.Phong != null)
        //            {
        //                decimal correctPrice = await pricingService.GetTotalRoomPriceAsync(ct.Phong.LoaiPhongId, dp.NgayNhanPhongDuKien, dp.NgayTraPhongDuKien);
        //                ct.GiaThucTe = correctPrice;
        //                newTotal += correctPrice;
        //            }
        //        }

        //        if (dp.HoaDon != null)
        //        {
        //            dp.HoaDon.TongTienBooking = newTotal;
        //            var ptGoc = dp.HoaDon.ChiTietHoaDons.FirstOrDefault(c => c.ChiTietDatPhongId == null);
        //            if (ptGoc != null)
        //            {
        //                ptGoc.SoTienPhaiTra = newTotal;
        //            }
        //        }
        //        fixedCount++;
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok(new { Message = $"Đã quét và sửa thành công giá tiền cho {fixedCount} đơn đặt phòng!" });
        //}
        // BƯỚC 5: Trực buồng dọn phòng xong (Đổi từ Trống bẩn -> Sẵn sàng)
        [HttpPut("rooms/{phongId}/mark-as-clean")]
        // [Authorize(Policy = AppPermissions.Housekeeping.UpdateStatus)] 
        [AllowAnonymous] // Đang để test
        public async Task<IActionResult> MarkRoomAsClean(Guid phongId)
        {
            try
            {
                await _bookingRepo.MarkRoomAsCleanAsync(phongId);
                return Ok(new { Message = "Đã dọn phòng xong. Phòng đã sẵn sàng đón khách mới!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi cập nhật trạng thái dọn phòng.",
                    Details = ex.Message
                });
            }
        }
    }

}