using BookingRoom.ViewModels;

namespace BookingRoom.Services
{
    public interface IPricingService
    {
        // Chỉ tính tổng giá  - Dùng cho màn hình tìm phòng
        Task<decimal> GetTotalRoomPriceAsync(Guid loaiPhongId, DateTime checkIn, DateTime checkOut);

        //  Lấy chi tiết lịch sử giá  - Dùng khi click "Xem chi tiết"
        Task<RoomTypePriceDetailVM> GetRoomPricingDetailsAsync(Guid loaiPhongId, DateTime checkIn, DateTime checkOut);
        //Lấy list giá phục vụ admin
        Task<List<RoomTypePriceDetailVM>> GetPriceMatrixAsync(DateTime startDate, DateTime endDate, Guid? loaiPhongId = null);
    }
}
