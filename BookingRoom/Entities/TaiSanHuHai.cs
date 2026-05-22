namespace BookingRoom.Entities
{
    public class TaiSanHuHai : BaseEntity
    {
        public Guid ChiTietDatPhongId { get; set; }
        public Guid ThietBiPhongId { get; set; }
        public Guid? TrucBuongId { get; set; } // Người phát hiện
        public string MoTaThietHai { get; set; } = null!;
        public decimal PhiDenBu { get; set; }

        public virtual ChiTietDatPhong ChiTietDatPhong { get; set; } = null!;
        public virtual ThietBiPhong ThietBiPhong { get; set; } = null!;
        public virtual TrucBuong? TrucBuong { get; set; }
    }
}
