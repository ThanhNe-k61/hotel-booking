namespace BookingRoom.ViewModels
{
    // Dành riêng cho API: Chi tiết giá một loại phòng (Xem chi tiết)
    public class RoomTypePriceDetailVM
    {
        public Guid LoaiPhongId { get; set; }
        public string TenLoaiPhong { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; } // Chỉ tính tổng trong khoảng checkIn -> checkOut
        public List<PricePerNightVM> PriceDetails { get; set; } = new List<PricePerNightVM>();
    }
    //Hiển thị giá
    public class PricePerNightVM
    {
        public DateTime Date { get; set; }
        public decimal BasePrice { get; set; }
        public decimal ActualPrice { get; set; }
        public List<string> AppliedPolicies { get; set; } = new List<string>();
    }
    //Tìm phòng trống
    public class AvailableRoomTypeVM 
    {


        public Guid LoaiPhongId { get; set; }
        public string TenLoaiPhong { get; set; } = string.Empty;
        public decimal RoomPrice { get; set; }
        public decimal TotalPrice { get; set; } //  tổng tiền gọn nhẹ, không chứa List chi tiết giá
        public decimal BasePrice { get; set; } // Giá gốc (không áp dụng chính sách)
        public int AvailableCount { get; set; }
        public List<AvailableRoomVM> Rooms { get; set; } = new List<AvailableRoomVM>();
    }
    public class AvailableRoomVM
    {
        public Guid PhongId { get; set; }
        public string SoPhong { get; set; } = string.Empty;
        public int Tang { get; set; }
    }
    //Booking
    public class CreateBookingVM
    {
        public Guid KhachHangId { get; set; }
        public DateTime NgayNhanPhongDuKien { get; set; }
        public DateTime NgayTraPhongDuKien { get; set; }
        public List<BookingDetailVM> RoomDetails { get; set; } = new List<BookingDetailVM>();
    }
    public class BookingDetailVM
    {
        public Guid PhongId { get; set; }
        public decimal GiaThucTe { get; set; }
    }
    public class AddServiceUsageVM
    {
        public Guid ChiTietDatPhongId { get; set; } 
        public Guid DichVuId { get; set; } 
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; } // Frontend tự nhân (Số lượng * Đơn giá) rồi gửi lên
    }

}
