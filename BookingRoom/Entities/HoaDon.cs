namespace BookingRoom.Entities
{
    public class HoaDon : BaseEntity
    {
        public Guid DatPhongId { get; set; }

        // Tổng tiền dự kiến của toàn bộ Booking (Phòng + Phát sinh)
        public decimal TongTienBooking { get; set; } = 0;

        // Sẽ tự động chuyển thành "Đã thanh toán" khi các bảng con đóng đủ tiền
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";

        public virtual DatPhong DatPhong { get; set; } = null!;
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}