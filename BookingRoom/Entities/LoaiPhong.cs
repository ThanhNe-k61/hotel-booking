namespace BookingRoom.Entities
{
    public class LoaiPhong : BaseEntity
    {
        public string TenLoaiPhong { get; set; } = null!;
        public decimal GiaCoBan { get; set; }
        public int SoNguoiToiDa { get; set; } = 2;
        public string? AnhDaiDien { get; set; }
        public string? MoTa { get; set; }

        public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
        public virtual ICollection<ChinhSachGia> ChinhSachGias { get; set; } = new List<ChinhSachGia>();
    }
}