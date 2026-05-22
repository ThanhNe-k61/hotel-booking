namespace BookingRoom.ViewModels
{
    // Dùng cho Create và Update (Gửi từ Client lên)
    public class LoaiPhongVM
    {
        public string TenLoaiPhong { get; set; } = string.Empty;
        public decimal GiaCoBan { get; set; }
        public int SoNguoiToiDa { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? MoTa { get; set; }
    }

    // Dùng cho GET (Trả từ Server về, có thêm Id)
    public class LoaiPhongResponseVM : LoaiPhongVM
    {
        public Guid Id { get; set; } 
    }
}