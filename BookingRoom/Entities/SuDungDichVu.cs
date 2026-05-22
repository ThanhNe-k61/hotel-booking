namespace BookingRoom.Entities
{
    public class SuDungDichVu : BaseEntity
    {
        public Guid ChiTietDatPhongId { get; set; }
        public Guid DichVuId { get; set; }
        public int SoLuong { get; set; } = 1;
        public decimal ThanhTien { get; set; }

        public virtual ChiTietDatPhong ChiTietDatPhong { get; set; } = null!;
        public virtual DichVu DichVu { get; set; } = null!;
    }
}
