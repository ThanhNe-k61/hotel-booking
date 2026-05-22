using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
namespace BookingRoom.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPricingService _pricingService;

        public BookingRepository(ApplicationDbContext context, IPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        // Lấy danh sách phòng trống
        public async Task<List<AvailableRoomTypeVM>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var availableRooms = await _context.Phongs
                                    .AsNoTracking()
                                    .Include(p => p.LoaiPhong)
                                    .Where(p => p.TrangThai != "Bảo trì" &&
                                        !_context.ChiTietDatPhongs.Any(c =>
                                        c.PhongId == p.Id &&
                                        c.DatPhong.TrangThai != "Đã hủy" &&
                                        c.DatPhong.TrangThai != "No-Show" &&
                                        // Ngày nhận thực tế hoặc dự kiến < Ngày trả của khách mới
                                        (c.ThoiGianNhanPhongThucTe != null ? c.ThoiGianNhanPhongThucTe.Value.Date : c.DatPhong.NgayNhanPhongDuKien.Date) < endDate.Date &&
                                        // Ngày trả thực tế (ưu tiên) hoặc dự kiến > Ngày nhận của khách mới
                                        (c.ThoiGianTraPhongThucTe != null ? c.ThoiGianTraPhongThucTe.Value.Date : c.DatPhong.NgayTraPhongDuKien.Date) > startDate.Date))
                                    .ToListAsync();

                var grouped = availableRooms.GroupBy(p => p.LoaiPhongId).ToList();
                var resultList = new List<AvailableRoomTypeVM>();
                int soDem = (int)(endDate.Date - startDate.Date).TotalDays;
                if (soDem <= 0) soDem = 1; 
                foreach (var group in grouped)
                {
                    var loaiPhong = group.FirstOrDefault()?.LoaiPhong;
                    decimal roomPrice = loaiPhong.GiaCoBan;
                    decimal basePrice = roomPrice * soDem;
                    var tongTien = await _pricingService.GetTotalRoomPriceAsync(loaiPhong.Id, startDate, endDate);
                    resultList.Add(new AvailableRoomTypeVM
                    {
                        LoaiPhongId = loaiPhong.Id,
                        TenLoaiPhong = loaiPhong.TenLoaiPhong,
                        RoomPrice = roomPrice,
                        TotalPrice = tongTien,
                        BasePrice = basePrice,
                        AvailableCount = group.Count(),
                        Rooms = group.Select(p => new AvailableRoomVM
                        {
                            PhongId = p.Id,
                            SoPhong = p.SoPhong,
                            Tang = p.Tang
                        }).ToList()
                    });
                }
                return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy phòng trống: {ex.Message}");
            }
        }

        //Đặt phòng 
        public async Task<Guid> CreateBookingAsync(CreateBookingVM request)
        {
            if (request.RoomDetails == null || !request.RoomDetails.Any())
                throw new Exception("Vui lòng chọn ít nhất một phòng!");

            if (request.NgayNhanPhongDuKien >= request.NgayTraPhongDuKien)
                throw new Exception("Ngày trả phòng phải sau ngày nhận phòng!");

            var roomIds = request.RoomDetails.Select(r => r.PhongId).ToList();
            if (roomIds.Count != roomIds.Distinct().Count())
                throw new Exception("Danh sách phòng chứa bản sao!");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fix logic check xung đột: Bắt theo ngày trả phòng thực tế
                var conflictRooms = await _context.ChiTietDatPhongs
                    .Include(c => c.DatPhong)
                    .Where(c => c.PhongId != null && roomIds.Contains(c.PhongId.Value) &&
                                c.DatPhong.TrangThai != "Đã hủy" &&
                                c.DatPhong.TrangThai != "No-Show" &&
                                (c.ThoiGianNhanPhongThucTe != null ? c.ThoiGianNhanPhongThucTe.Value.Date : c.DatPhong.NgayNhanPhongDuKien.Date) < request.NgayTraPhongDuKien.Date &&
                                (c.ThoiGianTraPhongThucTe != null ? c.ThoiGianTraPhongThucTe.Value.Date : c.DatPhong.NgayTraPhongDuKien.Date) > request.NgayNhanPhongDuKien.Date)
                    .Select(c => c.PhongId)
                    .ToListAsync();

                if (conflictRooms.Any())
                    throw new Exception("Rất tiếc, một số phòng bạn chọn vừa bị khách khác đặt mất. Vui lòng chọn lại!");

                var datPhong = new DatPhong
                {
                    KhachHangId = request.KhachHangId,
                    NgayNhanPhongDuKien = request.NgayNhanPhongDuKien,
                    NgayTraPhongDuKien = request.NgayTraPhongDuKien,
                    TrangThai = "Chờ xác nhận"
                };
                _context.DatPhongs.Add(datPhong);

                decimal tongTienPhong = 0;
                foreach (var item in request.RoomDetails)
                {
                    _context.ChiTietDatPhongs.Add(new ChiTietDatPhong
                    {
                        DatPhongId = datPhong.Id,
                        PhongId = item.PhongId,
                        GiaThucTe = item.GiaThucTe
                    });
                    tongTienPhong += item.GiaThucTe;
                }

                //  Vì đã đóng 100% tiền phòng, Hóa Đơn Cha và Phiếu thu đều là "Đã thanh toán"
                var hoaDon = new HoaDon
                {
                    DatPhongId = datPhong.Id,
                    TongTienBooking = tongTienPhong,
                    TrangThaiThanhToan = "Đã thanh toán" // Khởi tạo ban đầu là Đã thanh toán
                };
                _context.HoaDons.Add(hoaDon);

                _context.ChiTietHoaDons.Add(new ChiTietHoaDon
                {
                    HoaDonId = hoaDon.Id,
                    ChiTietDatPhongId = null, // Đại diện cho toàn bộ phòng
                    SoTienPhaiTra = tongTienPhong,
                    SoTienDaThanhToan = tongTienPhong, // Khách trả 100% tiền phòng
                    TrangThaiThanhToan = "Đã thanh toán",
                    NgayThanhToan = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return datPhong.Id;
            }
            catch (DbUpdateException dbEx) // bắt lỗi database cụ thể 
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu Database: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi hệ thống khi đặt phòng: {ex.Message}");
            }
        }

        //check-in
        public async Task<bool> CheckInAsync(Guid datPhongId, CheckInVM request)
        {
            if (request?.DanhSachPhong == null || !request.DanhSachPhong.Any())
                throw new Exception("Danh sách phòng check-in không được để trống!");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var datPhong = await _context.DatPhongs
                .Include(d => d.ChiTietDatPhongs)
                .FirstOrDefaultAsync(d => d.Id == datPhongId);

                if (datPhong == null) throw new Exception("Đặt phòng không tồn tại");
                //Lấy sẵn tất cả các Phòng cần cập nhật lên RAM một lần duy nhất
                var roomIdsToCheckIn = datPhong.ChiTietDatPhongs.Select(c => c.PhongId).ToList();
                var listPhongs = await _context.Phongs.Where(p => roomIdsToCheckIn.Contains(p.Id)).ToListAsync();

                foreach (var roomReq in request.DanhSachPhong)
                {
                    var chiTiet = datPhong.ChiTietDatPhongs.FirstOrDefault(c => c.Id == roomReq.ChiTietDatPhongId);
                    if (chiTiet != null)
                    {
                        chiTiet.ThoiGianNhanPhongThucTe = DateTime.UtcNow;
                        var phong = listPhongs.FirstOrDefault(p => p.Id == chiTiet.PhongId);
                        if (phong != null) phong.TrangThai = "Đang ở";

                        if (roomReq.KhachLuuTrus != null && roomReq.KhachLuuTrus.Any())
                        {
                            foreach (var guestReq in roomReq.KhachLuuTrus)
                            {
                                _context.KhachLuuTrus.Add(new KhachLuuTru
                                {
                                    ChiTietDatPhongId = chiTiet.Id,
                                    HoTen = guestReq.HoTen,
                                    CCCD_Passport = guestReq.CCCD_Passport,
                                    QuocTich = guestReq.QuocTich,
                                    ThoiGianCheckIn = DateTime.UtcNow
                                });
                            }
                        }
                    }
                }

                datPhong.TrangThai = "Đã nhận phòng";
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu Database: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi hệ thống khi check-in: {ex.Message}");
            }

        }
        //Khoá phòng để trực buồng kiểm tra
        public async Task<bool> RequestCheckoutInspectionAsync(Guid phongId)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(phongId);
                if (phong == null) throw new Exception("Phòng không tồn tại");
                phong.TrangThai = "Yêu cầu kiểm tra";
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Lỗi lưu Database: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi hệ thống khi yêu cầu kiểm tra: {ex.Message}");
            }
        }

        public async Task<bool> SubmitHousekeepingInspectionAsync(HousekeepingInspectVM request, Guid trucBuongId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var chiTiet = await _context.ChiTietDatPhongs
                                    .Include(c => c.DatPhong)
                                        .ThenInclude(d => d.HoaDon)
                                            .ThenInclude(h => h.ChiTietHoaDons)
                                    .FirstOrDefaultAsync(c => c.Id == request.ChiTietDatPhongId);

                if (chiTiet == null) throw new Exception("Không tìm thấy chi tiết đặt phòng.");
                if (chiTiet.DatPhong == null) throw new Exception("Dữ liệu Booking bị lỗi (Null DatPhong).");
                if (chiTiet.DatPhong.HoaDon == null) throw new Exception("Booking này chưa có Hóa Đơn gốc.");
                if (chiTiet.DatPhong.HoaDon.ChiTietHoaDons == null) throw new Exception("Hóa Đơn chưa có danh sách chi tiết hóa đơn.");
                
                decimal tongPhatSinhNay = 0;

                //  Lưu dịch vụ sử dụng 
                foreach (var svc in request.DichVuSuDung)
                {
                    _context.SuDungDichVus.Add(new SuDungDichVu { ChiTietDatPhongId = chiTiet.Id, DichVuId = svc.DichVuId, SoLuong = svc.SoLuong, ThanhTien = svc.ThanhTien });
                    tongPhatSinhNay += svc.ThanhTien;
                }

                //  Lưu tài sản hư hại
                foreach (var dmg in request.TaiSanHuHai)
                {
                    _context.TaiSanHuHais.Add(new TaiSanHuHai { ChiTietDatPhongId = chiTiet.Id, ThietBiPhongId = dmg.ThietBiPhongId, MoTaThietHai = dmg.MoTaThietHai, PhiDenBu = dmg.PhiDenBu, TrucBuongId = trucBuongId });
                    tongPhatSinhNay += dmg.PhiDenBu;
                }

                //  Phạt check-out muộn
                if (DateTime.UtcNow > chiTiet.DatPhong.NgayTraPhongDuKien.AddHours(1))
                {
                    var lateFeeSvc = await _context.DichVus.FirstOrDefaultAsync(d => d.TenDichVu.Contains("CheckOut Muộn"));
                    if (lateFeeSvc != null)
                    {
                        _context.SuDungDichVus.Add(new SuDungDichVu { ChiTietDatPhongId = chiTiet.Id, DichVuId = lateFeeSvc.Id, SoLuong = 1, ThanhTien = lateFeeSvc.Gia });
                        tongPhatSinhNay += lateFeeSvc.Gia;
                    }
                }

                //  Vì có phát sinh tiền chưa thu, Hóa Đơn Cha bị giáng cấp về "Chưa thanh toán"
                if (tongPhatSinhNay > 0 && chiTiet.DatPhong.HoaDon != null)
                {
                    var hoaDonCha = chiTiet.DatPhong.HoaDon;
                    hoaDonCha.TrangThaiThanhToan = "Chưa thanh toán";
                    // Mặc định chưa tách bill -> Tìm Phiếu thu gốc (ChiTietDatPhongId == null)
                    var phieuThuGoc = hoaDonCha.ChiTietHoaDons.FirstOrDefault(c => c.ChiTietDatPhongId == null);
                    if (phieuThuGoc != null)
                    {
                        phieuThuGoc.SoTienPhaiTra += tongPhatSinhNay; // Đôn số tiền phải trả lên
                        phieuThuGoc.TrangThaiThanhToan = "Chưa thanh toán"; // Đánh dấu nợ
                    }
                }

                var phong = await _context.Phongs.FindAsync(chiTiet.PhongId);
                if (phong != null) phong.TrangThai = "Trống bẩn";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu báo cáo buồng: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi hệ thống xử lý buồng phòng: {ex.Message}");
            }
        }

        //lấy hoá đơn tách hoá đơn
        public async Task<FolioInvoiceVM> GetInvoiceDetailsAsync(Guid datPhongId)
        {
            try
            {
                var datPhong = await _context.DatPhongs
                    .Include(d => d.HoaDon).ThenInclude(h => h.ChiTietHoaDons)
                    .Include(d => d.ChiTietDatPhongs)
                    .FirstOrDefaultAsync(d => d.Id == datPhongId);

                if (datPhong == null) throw new Exception("Booking không tồn tại.");
                var hoaDonGoc = datPhong.HoaDon;
                if (hoaDonGoc == null) throw new Exception("Không tìm thấy Hóa Đơn cho Booking này.");
                if (datPhong.ChiTietDatPhongs == null) throw new Exception("Dữ liệu Booking bị lỗi (Thiếu danh sách phòng).");
                if (hoaDonGoc.ChiTietHoaDons == null) throw new Exception("Dữ liệu Hóa đơn bị lỗi (Thiếu chi tiết hóa đơn).");

                var chiTietIds = datPhong.ChiTietDatPhongs.Select(c => c.Id).ToList();

                decimal totalServices = await _context.SuDungDichVus.Where(s => chiTietIds.Contains(s.ChiTietDatPhongId)).SumAsync(s => s.ThanhTien);
                decimal totalDamages = await _context.TaiSanHuHais.Where(t => chiTietIds.Contains(t.ChiTietDatPhongId)).SumAsync(t => t.PhiDenBu);

                return new FolioInvoiceVM
                {
                    DatPhongId = datPhong.Id,
                    HoaDonId = hoaDonGoc.Id,
                    TienPhong = hoaDonGoc.TongTienBooking, // Đây là tổng tiền phòng ban đầu
                    TienDichVu = totalServices,
                    TienDenBu = totalDamages,
                    //  Cứ tính tổng tất cả những gì đã thu (Kể cả tiền phòng lúc cọc và tiền Lễ tân thu lắt nhắt sau này)
                    TienDaCoc = hoaDonGoc.ChiTietHoaDons.Sum(c => c.SoTienDaThanhToan),
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất hóa đơn: {ex.Message}");
            }
        }
        //tách hoá đơn
        public async Task<Guid> SplitInvoiceByRoomAsync(SplitBillByRoomVM request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDonCha = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .FirstOrDefaultAsync(h => h.DatPhongId == request.DatPhongId);
                if (hoaDonCha == null) throw new Exception("Không tìm thấy hóa đơn tổng.");

                var chiTietPhong = await _context.ChiTietDatPhongs.FindAsync(request.ChiTietDatPhongId);
                if (chiTietPhong == null) throw new Exception("Phòng không tồn tại trong Booking này.");

                var tienDichVu = await _context.SuDungDichVus.Where(s => s.ChiTietDatPhongId == request.ChiTietDatPhongId).SumAsync(s => s.ThanhTien);
                var tienDenBu = await _context.TaiSanHuHais.Where(t => t.ChiTietDatPhongId == request.ChiTietDatPhongId).SumAsync(t => t.PhiDenBu);

                decimal tongTienPhongNay = chiTietPhong.GiaThucTe + tienDichVu + tienDenBu;

                // Tách phiếu thu con
                var newChiTietHD = new ChiTietHoaDon
                {
                    HoaDonId = hoaDonCha.Id,
                    ChiTietDatPhongId = request.ChiTietDatPhongId,
                    LeTanId = request.LeTanId,
                    SoTienPhaiTra = tongTienPhongNay,
                    SoTienDaThanhToan = 0, // Khởi tạo 0, lát nữa sẽ cộng tiền cọc dồn sang
                    TrangThaiThanhToan = "Chưa thanh toán"
                };

                var phieuThuGoc = hoaDonCha.ChiTietHoaDons.FirstOrDefault(c => c.ChiTietDatPhongId == null);
                if (phieuThuGoc != null)
                {
                    // Rút công nợ Phải Trả
                    phieuThuGoc.SoTienPhaiTra -= tongTienPhongNay;

                    // Rút tiền cọc Đã Thu (Chặn lỗi âm quỹ nếu khách chưa đóng đủ)
                    decimal tienCocChuyenSang = Math.Min(phieuThuGoc.SoTienDaThanhToan, chiTietPhong.GiaThucTe);
                    phieuThuGoc.SoTienDaThanhToan -= tienCocChuyenSang;
                    newChiTietHD.SoTienDaThanhToan += tienCocChuyenSang;

                    // Chốt trạng thái Bill Mẹ sau khi rút ruột
                    if (phieuThuGoc.SoTienPhaiTra <= phieuThuGoc.SoTienDaThanhToan)
                        phieuThuGoc.TrangThaiThanhToan = "Đã thanh toán";
                    else
                        phieuThuGoc.TrangThaiThanhToan = "Chưa thanh toán";
                }

                // Chốt trạng thái Bill Con
                if (newChiTietHD.SoTienPhaiTra <= newChiTietHD.SoTienDaThanhToan)
                    newChiTietHD.TrangThaiThanhToan = "Đã thanh toán";

                _context.ChiTietHoaDons.Add(newChiTietHD);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return newChiTietHD.Id;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu Database: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            { 
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi tách hóa đơn: {ex.Message}");
            }
        }
        // checkout (Đã nâng cấp để có thể Check-out 1 phòng hoặc toàn bộ)
        public async Task<bool> ConfirmPaymentAndCheckoutAsync(Guid datPhongId, string phuongThucTT, Guid leTanId, Guid? chiTietDatPhongId = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var datPhong = await _context.DatPhongs
                    .Include(d => d.ChiTietDatPhongs).ThenInclude(c => c.KhachLuuTrus)
                    .Include(d => d.ChiTietDatPhongs).ThenInclude(c => c.Phong)
                    .Include(d => d.HoaDon).ThenInclude(h => h.ChiTietHoaDons)
                    .FirstOrDefaultAsync(d => d.Id == datPhongId);

                if (datPhong == null) throw new Exception("Booking không tồn tại.");
                var hoaDonCha = datPhong.HoaDon;
                if (hoaDonCha == null) throw new Exception("Lỗi hệ thống: Không tìm thấy Hóa Đơn.");
                if (datPhong.ChiTietDatPhongs == null) throw new Exception("Dữ liệu Booking bị lỗi (Thiếu danh sách phòng).");
                if (hoaDonCha.ChiTietHoaDons == null) throw new Exception("Dữ liệu Hóa đơn bị lỗi (Thiếu chi tiết hóa đơn).");

                // Nếu truyền chiTietDatPhongId thì chỉ quét 1 phòng đó, nếu không thì quét toàn bộ
                var dsPhongCanCheckout = chiTietDatPhongId.HasValue
                    ? datPhong.ChiTietDatPhongs.Where(c => c.Id == chiTietDatPhongId.Value).ToList()
                    : datPhong.ChiTietDatPhongs.Where(c => c.ThoiGianTraPhongThucTe == null).ToList();

                foreach (var chiTiet in dsPhongCanCheckout)
                {
                    chiTiet.ThoiGianTraPhongThucTe = DateTime.UtcNow;

                    if (chiTiet.Phong != null)
                        chiTiet.Phong.TrangThai = "Trống bẩn";

                    foreach (var khach in chiTiet.KhachLuuTrus.Where(k => k.ThoiGianCheckOut == null))
                        khach.ThoiGianCheckOut = DateTime.UtcNow;
                }

                // Tương tự, chỉ thu tiền những tờ Hóa đơn con liên quan đến các phòng đang checkout
                var dsHoaDonCanThu = chiTietDatPhongId.HasValue
                    ? hoaDonCha.ChiTietHoaDons.Where(c => c.ChiTietDatPhongId == chiTietDatPhongId.Value).ToList()
                    : hoaDonCha.ChiTietHoaDons.ToList(); // Checkout toàn bộ thì thu cả gốc lẫn con

                foreach (var cthd in dsHoaDonCanThu)
                {
                    if (cthd.TrangThaiThanhToan != "Đã thanh toán")
                    {
                        cthd.SoTienDaThanhToan = cthd.SoTienPhaiTra;
                        cthd.TrangThaiThanhToan = "Đã thanh toán";
                        cthd.NgayThanhToan = DateTime.UtcNow;
                        cthd.PhuongThucThanhToan = phuongThucTT;
                        cthd.LeTanId = leTanId;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu Database khi checkout: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi hệ thống khi thanh toán: {ex.Message}");
            }
        }
        //Khách gọi thêm dịch vụ trong quá trình ở
        public async Task<bool> AddServiceToRoomAsync(AddServiceUsageVM request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var chiTiet = await _context.ChiTietDatPhongs
                    .Include(c => c.DatPhong)
                        .ThenInclude(d => d.HoaDon)
                            .ThenInclude(h => h.ChiTietHoaDons)
                    .FirstOrDefaultAsync(c => c.Id == request.ChiTietDatPhongId);

                if (chiTiet == null) throw new Exception("Không tìm thấy chi tiết đặt phòng.");
                if (chiTiet.DatPhong == null) throw new Exception("Dữ liệu Booking bị lỗi (Null DatPhong).");
                if (chiTiet.DatPhong.HoaDon == null) throw new Exception("Booking này chưa có Hóa Đơn gốc.");
                if (chiTiet.DatPhong.HoaDon.ChiTietHoaDons == null) throw new Exception("Hóa Đơn chưa có danh sách chi tiết hóa đơn.");

                //  Lưu lịch sử gọi món vào SuDungDichVu
                _context.SuDungDichVus.Add(new SuDungDichVu
                {
                    ChiTietDatPhongId = request.ChiTietDatPhongId,
                    DichVuId = request.DichVuId,
                    SoLuong = request.SoLuong,
                    ThanhTien = request.ThanhTien
                });

                //chưa tách bill update bill gốc
                if (request.ThanhTien > 0 && chiTiet.DatPhong.HoaDon != null)
                {
                    var hoaDonCha = chiTiet.DatPhong.HoaDon;
                    hoaDonCha.TrangThaiThanhToan = "Chưa thanh toán";
                    var phieuThuGoc = hoaDonCha.ChiTietHoaDons.FirstOrDefault(c => c.ChiTietDatPhongId == null);

                    if (phieuThuGoc != null)
                    {
                        phieuThuGoc.SoTienPhaiTra += request.ThanhTien; 
                        phieuThuGoc.TrangThaiThanhToan = "Chưa thanh toán"; 
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi lưu Database khi thêm dịch vụ: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi hệ thống khi thêm dịch vụ: {ex.Message}");
            }
        }
        // Cập nhật phòng từ Trống bẩn -> Sẵn sàng
        public async Task<bool> MarkRoomAsCleanAsync(Guid phongId)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(phongId);
                if (phong == null)
                    throw new Exception("Không tìm thấy phòng.");

                // Chỉ cho phép đổi sang Sẵn sàng nếu phòng đang Trống bẩn
                if (phong.TrangThai != "Trống bẩn")
                    throw new Exception("Phòng này không ở trạng thái cần dọn dẹp!");

                phong.TrangThai = "Sẵn sàng";

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Lỗi lưu Database khi cập nhật trạng thái phòng: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi hệ thống khi cập nhật trạng thái phòng: {ex.Message}");
            }
        }
    }
}