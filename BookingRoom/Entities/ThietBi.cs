namespace BookingRoom.Entities
{
    public class ThietBi : BaseEntity
    {
        public string TenThietBi { get; set; } = null!;
        public int TongSoLuong { get; set; } = 0;
        public string Icon { get; set; } = "TV";
        public virtual ICollection<ThietBiPhong> ThietBiPhongs { get; set; } = new List<ThietBiPhong>();
    }
}
