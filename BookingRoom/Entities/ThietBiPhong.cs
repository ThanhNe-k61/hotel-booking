namespace BookingRoom.Entities
{
    public class ThietBiPhong : BaseEntity
    {
        public Guid PhongId { get; set; }
        public Guid ThietBiId { get; set; }
        public string TinhTrang { get; set; } = "Tốt";

        public virtual Phong Phong { get; set; } = null!;
        public virtual ThietBi ThietBi { get; set; } = null!;
        public virtual ICollection<TaiSanHuHai> TaiSanHuHais { get; set; } = new List<TaiSanHuHai>();
    }
}
