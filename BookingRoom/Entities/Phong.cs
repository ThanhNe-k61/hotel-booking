namespace BookingRoom.Entities
{
    public class Phong : BaseEntity
    {
        public Guid LoaiPhongId { get; set; }
        public string SoPhong { get; set; } = null!;
        public int Tang { get; set; }
        public string TrangThai { get; set; } = "Sẵn sàng";

        public virtual LoaiPhong LoaiPhong { get; set; } = null!;
        public virtual ICollection<ThietBiPhong> ThietBiPhongs { get; set; } = new List<ThietBiPhong>();
        public virtual ICollection<ChiTietDatPhong> ChiTietDatPhongs { get; set; } = new List<ChiTietDatPhong>();
    }
}