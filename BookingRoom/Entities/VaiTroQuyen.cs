namespace BookingRoom.Entities
{
    public class VaiTroQuyen
    {
        public Guid VaiTroId { get; set; }
        public Guid QuyenId { get; set; }
        public virtual VaiTro VaiTro { get; set; } = null!;
        public virtual Quyen Quyen { get; set; } = null!;
    }
}