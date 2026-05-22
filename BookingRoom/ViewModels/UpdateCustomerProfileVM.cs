namespace BookingRoom.ViewModels
{
    public class UpdateCustomerProfileVM
    {
        public string HoTen { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? CccdPassport { get; set; }
        public string? QuocTich { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
    }
}
