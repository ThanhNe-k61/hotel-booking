namespace BookingRoom.Entities
{
    public class ChiTietHoaDon : BaseEntity
    {
        public Guid HoaDonId { get; set; }

        //  Nếu null -> Thu tiền chung cả đoàn. Nếu có ID -> Thu tiền riêng phòng đó.
        public Guid? ChiTietDatPhongId { get; set; }

        public Guid? LeTanId { get; set; } //Người thu

        public decimal SoTienPhaiTra { get; set; }
        public decimal SoTienDaThanhToan { get; set; } = 0;

        public string? PhuongThucThanhToan { get; set; }
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";
        public DateTime? NgayThanhToan { get; set; }

        public virtual HoaDon HoaDon { get; set; } = null!;
        public virtual ChiTietDatPhong? ChiTietDatPhong { get; set; }
        public virtual LeTan? LeTan { get; set; }
    }
}