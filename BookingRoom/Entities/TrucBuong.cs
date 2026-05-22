namespace BookingRoom.Entities
{
    public class TrucBuong : BaseEntity
    {
        public Guid NguoiDungId { get; set; }
        public string? KhuVucPhuTrach { get; set; }
        public DateTime NgayVaoLam { get; set; } = DateTime.UtcNow;
        public decimal? MucLuongCoBan { get; set; }
        public virtual NguoiDung NguoiDung { get; set; } = null!;
        public virtual ICollection<TaiSanHuHai> TaiSanHuHais { get; set; } = new List<TaiSanHuHai>();
    }
}
