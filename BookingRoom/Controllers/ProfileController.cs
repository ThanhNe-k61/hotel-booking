using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/profiles")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthRepository _authRepo;

    public ProfileController(ApplicationDbContext context, IAuthRepository authRepo)
    {
        _context = context;
        _authRepo = authRepo;
    }

    [Authorize(Policy = "System.Onboarding")] 
    [HttpPost]
    public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileVM model)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.NguoiDungs.FindAsync(userId);

            if (user == null || user.IsProfileCompleted)
                return BadRequest(new { Message = "Hồ sơ đã hoàn thiện hoặc lỗi tài khoản." });

            //cập nhật thông tin chung
            user.SoDienThoai = model.SoDienThoai;
            user.NgaySinh = model.NgaySinh;
            user.GioiTinh = model.GioiTinh;
            user.QueQuan = model.QueQuan;


            //dùng enum để xác định bảng nào cần insert thêm thông tin đặc thù
            switch (user.DefaultRole)
            {
                case UserRoleEnum.Customer:
                    _context.KhachHangs.Add(new KhachHang
                    {
                        NguoiDungId = userId,
                        CccdPassport = model.CCCD_Passport,
                        QuocTich = model.QuocTich,
                        SoDienThoai = model.SoDienThoai,
                        HoTen = user.HoTen,
                        NgaySinh = model.NgaySinh,
                        GioiTinh = model.GioiTinh,
                        QueQuan = model.QueQuan,
                        DiemTichLuy = 0
                    });
                    break;

                case UserRoleEnum.Receptionist:
                    _context.LeTans.Add(new LeTan
                    {
                        NguoiDungId = userId,
                        NgayVaoLam = DateTime.UtcNow
                    });
                    break;

                case UserRoleEnum.Housekeeper:
                    _context.TrucBuongs.Add(new TrucBuong
                    {
                        NguoiDungId = userId,
                        NgayVaoLam = DateTime.UtcNow
                    });
                    break;
            }

            // đánh dấu hoàn thành
            user.IsProfileCompleted = true;
            await _context.SaveChangesAsync();

            // Lấy lại thông tin quyền và vai trò để cấp token mới có đầy đủ claims
            var roleIds = await _context.NguoiDungVaiTros.Where(nv => nv.NguoiDungId == userId).Select(nv => nv.VaiTroId).ToListAsync();
            var roleNames = await _context.VaiTros.Where(vt => roleIds.Contains(vt.Id)).Select(vt => vt.TenVaiTro).ToListAsync();
            var permissions = await _context.VaiTroQuyens.Where(vq => roleIds.Contains(vq.VaiTroId)).Select(vq => vq.Quyen.GiaTriQuyen).Distinct().ToListAsync();

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email!)
        };
            foreach (var r in roleNames) authClaims.Add(new Claim(ClaimTypes.Role, r));
            foreach (var p in permissions) authClaims.Add(new Claim("Permission", p));

            var finalToken = _authRepo.GenerateJwtToken(authClaims);

            return Ok(new
            {
                Message = "Cập nhật thành công!",
                Token = new JwtSecurityTokenHandler().WriteToken(finalToken)
            });
        }
        catch (Exception ex)
        {
            return BadRequest (new
            {
                Message = "Lỗi cập nhật hồ sơ",
                Details = ex.Message
            });
        }
    }
    [HttpGet("customer/{khachHangId}")]
    [Authorize(Policy = AppPermissions.Profile.View)]
    public async Task<IActionResult> GetCustomerProfile(Guid khachHangId)
    {
        try
        {
            // Dùng Include để lôi thêm thông tin Email (từ NguoiDung) 
            var khachHang = await _context.KhachHangs
                .AsNoTracking() // Dùng AsNoTracking cho API chỉ đọc (GET) để tăng tốc độ
                .Include(k => k.NguoiDung)
                .FirstOrDefaultAsync(k => k.Id == khachHangId);

            if (khachHang == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông tin khách hàng." });
            }

            var profileResult = new
            {
                HoTen = khachHang.HoTen,
                Email = khachHang.NguoiDung?.Email, 
                SoDienThoai = khachHang.SoDienThoai,
                CccdPassport = khachHang.CccdPassport,
                QuocTich = khachHang.QuocTich,
                DiemTichLuy = khachHang.DiemTichLuy,
                NgaySinh = khachHang.NgaySinh,
                GioiTinh = khachHang.GioiTinh,
                QueQuan = khachHang.QueQuan,
            };

            return Ok(profileResult);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = "Đã xảy ra lỗi khi tải hồ sơ khách hàng.",
                Details = ex.Message
            });
        }
    }
    [HttpPut("customer/{khachHangId}")]
    [Authorize(Policy = AppPermissions.Profile.Edit)] 
    public async Task<IActionResult> UpdateCustomerProfile(Guid khachHangId, [FromBody] UpdateCustomerProfileVM request)
    {
        try
        {
            // Tìm Khách hàng và lôi luôn cái NguoiDung (Tài khoản gốc) lên để đồng bộ
            var khachHang = await _context.KhachHangs
                .Include(k => k.NguoiDung)
                .FirstOrDefaultAsync(k => k.Id == khachHangId);

            if (khachHang == null)
            {
                return NotFound(new { Message = "Không tìm thấy thông tin khách hàng." });
            }

            //  Cập nhật thông tin cho bảng KhachHangs
            khachHang.HoTen = request.HoTen;
            khachHang.SoDienThoai = request.SoDienThoai;
            khachHang.CccdPassport = request.CccdPassport;
            khachHang.QuocTich = request.QuocTich;
            khachHang.NgaySinh = request.NgaySinh;
            khachHang.GioiTinh = request.GioiTinh;
            khachHang.QueQuan = request.QueQuan;

            // Đẩy luôn dữ liệu mới sang bảng NguoiDungs để 2 bên khớp nhau 100%
            if (khachHang.NguoiDung != null)
            {
                khachHang.NguoiDung.HoTen = request.HoTen;
                khachHang.NguoiDung.SoDienThoai = request.SoDienThoai;
                khachHang.NguoiDung.NgaySinh = request.NgaySinh;
                khachHang.NguoiDung.GioiTinh = request.GioiTinh;
                khachHang.NguoiDung.QueQuan = request.QueQuan;
            }

            //  Lưu một phát ăn ngay cả 2 bảng
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cập nhật hồ sơ thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = "Đã xảy ra lỗi khi cập nhật hồ sơ khách hàng.",
                Details = ex.Message
            });
        }
    }
    [HttpGet("customer/{khachHangId}/bookings")]
    [Authorize(Policy = AppPermissions.Profile.View)]
    public async Task<IActionResult> GetBookingHistory(Guid khachHangId)
    {
        try
        {
            // Dùng .Select() map thẳng ra object ẩn danh, thay .Include()
            var history = await _context.DatPhongs
                .AsNoTracking()
                .Where(d => d.KhachHangId == khachHangId)
                .OrderByDescending(d => d.NgayNhanPhongDuKien) // Xếp mới nhất lên đầu
                .Select(d => new
                {
                    DatPhongId = d.Id,
                    NgayDat = d.CreatedAt, // Lấy từ BaseEntity
                    NgayNhanPhong = d.NgayNhanPhongDuKien,
                    NgayTraPhong = d.NgayTraPhongDuKien,
                    TrangThai = d.TrangThai,
                    // Kéo tổng tiền từ Hóa Đơn (Nếu chưa có hóa đơn thì để 0)
                    TongTien = d.HoaDon != null ? d.HoaDon.TongTienBooking : 0,
                    SoLuongPhong = d.ChiTietDatPhongs.Count,
                })
                .ToListAsync();

            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Đã xảy ra lỗi khi tải lịch sử đặt phòng.", Details = ex.Message });
        }
    }
    [HttpGet("bookings/{datPhongId}/details")]
    [Authorize]
    public async Task<IActionResult> GetBookingDetailHistory(Guid datPhongId)
    {
        try
        {
            var bookingDetail = await _context.DatPhongs
                .AsNoTracking()
                .Where(d => d.Id == datPhongId)
                .Select(d => new
                {
                    DatPhongId = d.Id,
                    NgayNhanPhongDuKien = d.NgayNhanPhongDuKien,
                    NgayTraPhongDuKien = d.NgayTraPhongDuKien,
                    TrangThai = d.TrangThai,
                    TongTienPhong = d.HoaDon != null ? d.HoaDon.TongTienBooking : 0,
                    TongDaThanhToan = d.HoaDon != null ? d.HoaDon.ChiTietHoaDons.Sum(c => c.SoTienDaThanhToan) : 0,

                    // Quét từng chi tiết phòng trong cái Booking này
                    Rooms = d.ChiTietDatPhongs.Select(c => new
                    {
                        ChiTietDatPhongId = c.Id,
                        SoPhong = c.Phong.SoPhong,
                        LoaiPhong = c.Phong.LoaiPhong.TenLoaiPhong,
                        GiaThucTe = c.GiaThucTe,
                        ThoiGianNhanThucTe = c.ThoiGianNhanPhongThucTe,
                        ThoiGianTraThucTe = c.ThoiGianTraPhongThucTe,

                        // Lồng sub-query để bóc Dịch Vụ của phòng này
                        DichVus = _context.SuDungDichVus
                            .Where(sd => sd.ChiTietDatPhongId == c.Id)
                            .Select(sd => new
                            {
                                TenDichVu = sd.DichVu.TenDichVu,
                                SoLuong = sd.SoLuong,
                                ThanhTien = sd.ThanhTien
                            }).ToList(),

                        // Lồng sub-query để bóc Tài Sản Hư Hại của phòng này
                        HuHais = _context.TaiSanHuHais
                            .Where(ts => ts.ChiTietDatPhongId == c.Id)
                            .Select(ts => new
                            {
                                TenThietBi = ts.ThietBiPhong.ThietBi.TenThietBi, // Lấy tên từ bảng ThietBiPhong
                                MoTa = ts.MoTaThietHai,
                                PhiDenBu = ts.PhiDenBu
                            }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (bookingDetail == null)
                return NotFound(new { Message = "Không tìm thấy thông tin chi tiết của lần đặt phòng này." });

            return Ok(bookingDetail);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Lỗi khi tải chi tiết hóa đơn đặt phòng.", Details = ex.Message });
        }
    }
}