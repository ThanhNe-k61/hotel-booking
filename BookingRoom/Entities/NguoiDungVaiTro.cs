namespace BookingRoom.Entities
{
    public class NguoiDungVaiTro
    {
        public Guid? NguoiDungId { get; set; }
        public Guid? VaiTroId { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; } = null!;
        public virtual VaiTro? VaiTro { get; set; } = null!;
    }
}