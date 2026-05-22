namespace BookingRoom.ViewModels
{
    public class HousekeepingInspectVM
    {
        public Guid ChiTietDatPhongId { get; set; }
        public List<ServiceUsageVM> DichVuSuDung { get; set; } = new(); 
        public List<DamageReportVM> TaiSanHuHai { get; set; } = new(); 
    }
    public class ServiceUsageVM
    {
        public Guid DichVuId { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
    }
    public class DamageReportVM
    {
        public Guid ThietBiPhongId { get; set; }
        public string MoTaThietHai { get; set; } = string.Empty;
        public decimal PhiDenBu { get; set; }
    }
}
