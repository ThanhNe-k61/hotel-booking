namespace BookingRoom.Entities
{
    public class DichVu : BaseEntity
    {
        public string TenDichVu { get; set; } = null!;
        public decimal Gia { get; set; }
        public string LoaiDichVu { get; set; } = "Khác";
        public string TrangThai { get; set; } = "Hoạt động";
        public string MoTa { get; set; } = null!;

        public virtual ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();
    }
}
