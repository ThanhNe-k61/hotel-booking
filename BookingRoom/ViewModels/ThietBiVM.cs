namespace BookingRoom.ViewModels
{
    // Dùng cho Quản lý Danh mục kho
    public class ThietBiVM
    {
        public string TenThietBi { get; set; } = string.Empty;
        public int TongSoLuong { get; set; }
        public string? Icon { get; set; }
    }

    // Dùng cho việc Phân bổ vào phòng
    public class AssignThietBiVM
    {
        public Guid PhongId { get; set; }
        public Guid ThietBiId { get; set; }
        public string TinhTrang { get; set; } = "Tốt";
    }

    // Dùng cho việc Update tình trạng
    public class UpdateTinhTrangVM
    {
        public string TinhTrang { get; set; } = string.Empty;
    }
}