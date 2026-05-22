namespace BookingRoom.Entities
{
    public class VaiTro: BaseEntity
    {
        public string TenVaiTro { get; set; } = null!;
        public bool DaXoa { get; set; } = false;
        public virtual ICollection<VaiTroQuyen> VaiTroQuyens { get; set; } = new List<VaiTroQuyen>();
        public virtual ICollection<NguoiDungVaiTro> NguoiDungVaiTros { get; set; } = new List<NguoiDungVaiTro>();
    }
}