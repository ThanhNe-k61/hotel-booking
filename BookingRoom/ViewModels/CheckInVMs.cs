namespace BookingRoom.ViewModels
{
    public class CheckInVM
    {
       public List<RoomGuestVM> DanhSachPhong { get; set; } = new (); 
    }
    public class RoomGuestVM
    {
        public Guid ChiTietDatPhongId { get; set; }
        public List<GuestInfoVM> KhachLuuTrus { get; set; } = new ();
    }
    public class GuestInfoVM
    {
        public string HoTen { get; set; } = string.Empty;
        public string CCCD_Passport { get; set; } = string.Empty;
        public string? QuocTich { get; set; }
    }
}
