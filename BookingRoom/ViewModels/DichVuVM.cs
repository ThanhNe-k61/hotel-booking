namespace BookingRoom.ViewModels
{
    public class DichVuVM
    {
        public string TenDichVu { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public string? MoTa { get; set; }
        public string LoaiDichVu { get; set; } = "Khác";
        public string TrangThai { get; set; } = "Hoạt động";
    }
}