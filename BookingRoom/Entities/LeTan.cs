namespace BookingRoom.Entities
{
    public class LeTan : BaseEntity
    {
        public Guid NguoiDungId { get; set; }
        public DateTime NgayVaoLam { get; set; } = DateTime.UtcNow;
        public decimal? MucLuongCoBan { get; set; }
        public virtual NguoiDung NguoiDung { get; set; } = null!;
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
