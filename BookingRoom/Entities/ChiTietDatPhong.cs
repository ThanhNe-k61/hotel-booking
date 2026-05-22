namespace BookingRoom.Entities
{
    public class ChiTietDatPhong : BaseEntity
    {
        public Guid DatPhongId { get; set; }
        public Guid? PhongId { get; set; } // NULL nếu chưa xếp phòng cụ thể
        public decimal GiaThucTe { get; set; }
        public DateTime? ThoiGianNhanPhongThucTe { get; set; }
        public DateTime? ThoiGianTraPhongThucTe { get; set; }

        public virtual DatPhong DatPhong { get; set; } = null!;
        public virtual Phong? Phong { get; set; }

        public virtual ICollection<KhachLuuTru> KhachLuuTrus { get; set; } = new List<KhachLuuTru>();
        public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();
        public virtual ICollection<TaiSanHuHai> TaiSanHuHais { get; set; } = new List<TaiSanHuHai>();
    }
}
