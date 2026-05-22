using BookingRoom.Data;
using BookingRoom.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Services
{
    public class PricingService : IPricingService
    {
        private readonly ApplicationDbContext _context;

        public PricingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRoomPriceAsync(Guid loaiPhongId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);
                if (loaiPhong == null) return 0;

                int totalNights = (int)(checkOut.Date - checkIn.Date).TotalDays;
                if (totalNights <= 0) totalNights = 1;

                var activePolicies = await _context.ChinhSachGias
                    .Where(cs => cs.TrangThai == "Kích hoạt" &&
                                (cs.LoaiPhongId == loaiPhongId || cs.LoaiPhongId == Guid.Empty))
                    .OrderByDescending(cs => cs.DoUuTien)
                    .ToListAsync();

                decimal totalPrice = 0;

                for (int i = 0; i < totalNights; i++)
                {
                    DateTime currentNight = checkIn.Date.AddDays(i);
                    string dayOfWeek = currentNight.DayOfWeek.ToString();
                    decimal actualPrice = loaiPhong.GiaCoBan;

                    var applicablePolicies = activePolicies.Where(cs =>
                        (!cs.TuNgay.HasValue || cs.TuNgay.Value.Date <= currentNight) &&
                        (!cs.DenNgay.HasValue || cs.DenNgay.Value.Date >= currentNight) &&
                        (cs.ApDungChoThu == "ALL" || cs.ApDungChoThu.Contains(dayOfWeek))
                    ).ToList();

                    foreach (var policy in applicablePolicies)
                    {
                        if (policy.LoaiDieuChinh == "Cộng thẳng") actualPrice += policy.GiaTriDieuChinh;
                        else if (policy.LoaiDieuChinh == "Trừ thẳng") actualPrice -= policy.GiaTriDieuChinh;
                        else if (policy.LoaiDieuChinh == "Cộng phần trăm") actualPrice += loaiPhong.GiaCoBan * (policy.GiaTriDieuChinh / 100m);
                        else if (policy.LoaiDieuChinh == "Trừ phần trăm") actualPrice -= loaiPhong.GiaCoBan * (policy.GiaTriDieuChinh / 100m);
                    }

                    totalPrice += actualPrice;
                }

                return totalPrice;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tính giá phòng: {ex.Message}");
            }
        }

        public async Task<RoomTypePriceDetailVM?> GetRoomPricingDetailsAsync(Guid loaiPhongId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);
                if (loaiPhong == null) return null;

                int totalNights = (int)(checkOut.Date - checkIn.Date).TotalDays;
                if (totalNights <= 0) totalNights = 1;

                var activePolicies = await _context.ChinhSachGias
                    .Where(cs => cs.TrangThai == "Kích hoạt" &&
                                (cs.LoaiPhongId == loaiPhongId || cs.LoaiPhongId == Guid.Empty))
                    .OrderByDescending(cs => cs.DoUuTien)
                    .ToListAsync();

                var result = new RoomTypePriceDetailVM
                {
                    LoaiPhongId = loaiPhong.Id,
                    TenLoaiPhong = loaiPhong.TenLoaiPhong,
                    TotalPrice = 0
                };

                for (int i = 0; i < totalNights; i++)
                {
                    DateTime currentNight = checkIn.Date.AddDays(i);
                    string dayOfWeek = currentNight.DayOfWeek.ToString();

                    var nightDetail = new PricePerNightVM
                    {
                        Date = currentNight,
                        BasePrice = loaiPhong.GiaCoBan,
                        ActualPrice = loaiPhong.GiaCoBan
                    };

                    var applicablePolicies = activePolicies.Where(cs =>
                        (!cs.TuNgay.HasValue || cs.TuNgay.Value.Date <= currentNight) &&
                        (!cs.DenNgay.HasValue || cs.DenNgay.Value.Date >= currentNight) &&
                        (cs.ApDungChoThu == "ALL" || cs.ApDungChoThu.Contains(dayOfWeek))
                    ).ToList();

                    foreach (var policy in applicablePolicies)
                    {
                        if (policy.LoaiDieuChinh == "Cộng thẳng")
                        {
                            nightDetail.ActualPrice += policy.GiaTriDieuChinh;
                            nightDetail.AppliedPolicies.Add(policy.TenChinhSach);
                        }
                        else if (policy.LoaiDieuChinh == "Trừ thẳng")
                        {
                            nightDetail.ActualPrice -= policy.GiaTriDieuChinh;
                            nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (-{policy.GiaTriDieuChinh})");
                        }
                        else if (policy.LoaiDieuChinh == "Cộng phần trăm")
                        {
                            decimal increaseAmount = nightDetail.BasePrice * (policy.GiaTriDieuChinh / 100m);
                            nightDetail.ActualPrice += increaseAmount;
                            nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (+{policy.GiaTriDieuChinh}%)");
                        }
                        else if (policy.LoaiDieuChinh == "Trừ phần trăm")
                        {
                            decimal decreaseAmount = nightDetail.BasePrice * (policy.GiaTriDieuChinh / 100m);
                            nightDetail.ActualPrice -= decreaseAmount;
                            nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (-{policy.GiaTriDieuChinh}%)");
                        }
                    }

                    result.PriceDetails.Add(nightDetail);
                    result.TotalPrice += nightDetail.ActualPrice;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết giá phòng: {ex.Message}");
            }
        }

        public async Task<List<RoomTypePriceDetailVM>> GetPriceMatrixAsync(DateTime startDate, DateTime endDate, Guid? loaiPhongId = null)
        {
            try
            {
                var resultList = new List<RoomTypePriceDetailVM>();
                int totalNights = (int)(endDate.Date - startDate.Date).TotalDays;
                if (totalNights <= 0) return resultList;

                var loaiPhongsQuery = _context.LoaiPhongs.AsQueryable();
                if (loaiPhongId.HasValue)
                    loaiPhongsQuery = loaiPhongsQuery.Where(lp => lp.Id == loaiPhongId.Value);

                var loaiPhongs = await loaiPhongsQuery.ToListAsync();

                var activePolicies = await _context.ChinhSachGias
                    .Where(cs => cs.TrangThai == "Kích hoạt" &&
                                (!cs.TuNgay.HasValue || cs.TuNgay.Value.Date <= endDate.Date) &&
                                (!cs.DenNgay.HasValue || cs.DenNgay.Value.Date >= startDate.Date))
                    .OrderByDescending(cs => cs.DoUuTien)
                    .ToListAsync();

                foreach (var lp in loaiPhongs)
                {
                    var roomResult = new RoomTypePriceDetailVM
                    {
                        LoaiPhongId = lp.Id,
                        TenLoaiPhong = lp.TenLoaiPhong,
                        TotalPrice = 0
                    };

                    for (int i = 0; i < totalNights; i++)
                    {
                        DateTime currentNight = startDate.Date.AddDays(i);
                        string dayOfWeek = currentNight.DayOfWeek.ToString();

                        var nightDetail = new PricePerNightVM
                        {
                            Date = currentNight,
                            BasePrice = lp.GiaCoBan,
                            ActualPrice = lp.GiaCoBan
                        };

                        var applicablePolicies = activePolicies.Where(cs =>
                            (cs.LoaiPhongId == lp.Id || cs.LoaiPhongId == Guid.Empty) &&
                            (!cs.TuNgay.HasValue || cs.TuNgay.Value.Date <= currentNight) &&
                            (!cs.DenNgay.HasValue || cs.DenNgay.Value.Date >= currentNight) &&
                            (cs.ApDungChoThu == "ALL" || cs.ApDungChoThu.Contains(dayOfWeek))
                        ).ToList();

                        foreach (var policy in applicablePolicies)
                        {
                            if (policy.LoaiDieuChinh == "Cộng thẳng")
                            {
                                nightDetail.ActualPrice += policy.GiaTriDieuChinh;
                                nightDetail.AppliedPolicies.Add(policy.TenChinhSach);
                            }
                            else if (policy.LoaiDieuChinh == "Trừ thẳng")
                            {
                                nightDetail.ActualPrice -= policy.GiaTriDieuChinh;
                                nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (-{policy.GiaTriDieuChinh})");
                            }
                            else if (policy.LoaiDieuChinh == "Cộng phần trăm")
                            {
                                decimal increaseAmount = nightDetail.BasePrice * (policy.GiaTriDieuChinh / 100m);
                                nightDetail.ActualPrice += increaseAmount;
                                nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (+{policy.GiaTriDieuChinh}%)");
                            }
                            else if (policy.LoaiDieuChinh == "Trừ phần trăm")
                            {
                                decimal decreaseAmount = nightDetail.BasePrice * (policy.GiaTriDieuChinh / 100m);
                                nightDetail.ActualPrice -= decreaseAmount;
                                nightDetail.AppliedPolicies.Add($"{policy.TenChinhSach} (-{policy.GiaTriDieuChinh}%)");
                            }
                        }

                        roomResult.PriceDetails.Add(nightDetail);
                        roomResult.TotalPrice += nightDetail.ActualPrice;
                    }
                    resultList.Add(roomResult);
                }

                return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy ma trận giá phòng: {ex.Message}");
            }
        }
    }
}