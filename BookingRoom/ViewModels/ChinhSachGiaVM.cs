namespace BookingRoom.ViewModels
{
    public class ChinhSachGiaVM
    {
        public Guid LoaiPhongId { get; set; } 
        public string TenChinhSach { get; set; } = null!;
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string ApDungChoThu { get; set; } = "ALL";
        public string LoaiDieuChinh { get; set; } = "Cộng thẳng";
        public decimal GiaTriDieuChinh { get; set; }
        public int DoUuTien { get; set; } = 1;
        public string TrangThai { get; set; } = "Kích hoạt";
    }
}