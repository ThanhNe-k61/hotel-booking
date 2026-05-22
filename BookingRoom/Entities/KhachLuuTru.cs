namespace BookingRoom.Entities
{
    public class KhachLuuTru : BaseEntity
    {
        public Guid ChiTietDatPhongId { get; set; }
        public string HoTen { get; set; } = null!;
        public string CCCD_Passport { get; set; } = null!;
        public string? QuocTich { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime ThoiGianCheckIn { get; set; } = DateTime.UtcNow;
        public DateTime? ThoiGianCheckOut { get; set; }

        public virtual ChiTietDatPhong ChiTietDatPhong { get; set; } = null!;
    }
}
