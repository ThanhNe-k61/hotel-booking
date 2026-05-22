namespace BookingRoom.ViewModels
{
    public class PhongVM
    {
        public Guid LoaiPhongId { get; set; }
        public string SoPhong { get; set; } = string.Empty;
        public int Tang { get; set; } 
        public string TrangThai { get; set; } = "Sẵn sàng";
    }

    public class PhongResponseVM : PhongVM
    {
        public Guid Id { get; set; }
        public string TenLoaiPhong { get; set; } = string.Empty;
    }
}