namespace BookingRoom.Entities
{
    public class DatPhong : BaseEntity
    {
        public Guid KhachHangId { get; set; } // Liên kết sang Khách Hàng
        public DateTime NgayDat { get; set; } = DateTime.UtcNow;
        public DateTime NgayNhanPhongDuKien { get; set; }
        public DateTime NgayTraPhongDuKien { get; set; }
        public string TrangThai { get; set; } = "Chờ xác nhận";

        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual ICollection<ChiTietDatPhong> ChiTietDatPhongs { get; set; } = new List<ChiTietDatPhong>();
        public virtual HoaDon? HoaDon { get; set; }
    }
}
