namespace BookingRoom.Entities
{
    public class Quyen: BaseEntity
    {
        public Guid ChucNangId { get; set; }
        public string TenQuyen { get; set; } = null!; // VD: "Create", "View"
        public string GiaTriQuyen { get; set; } = null!; // Bổ sung để lưu dạng "Booking.Create" giống Identity Claim
        public bool DaXoa { get; set; } = false;
        public virtual ChucNang ChucNang { get; set; } = null!;
        public virtual ICollection<VaiTroQuyen> VaiTroQuyens { get; set; } = new List<VaiTroQuyen>();
    }
}