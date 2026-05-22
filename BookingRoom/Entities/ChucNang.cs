namespace BookingRoom.Entities
{
    public class ChucNang:BaseEntity
    {
        public string TenChucNang { get; set; } = null!;
        public string? MoTa { get; set; }
        public bool DaXoa { get; set; } = false;
        public virtual ICollection<Quyen> Quyens { get; set; } = new List<Quyen>();
    }
}