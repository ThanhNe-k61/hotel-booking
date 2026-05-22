using BookingRoom.Entities;

public class KhachHang : BaseEntity
{
    public Guid NguoiDungId { get; set; }
    public string? CccdPassport { get; set; }
    public string? QuocTich { get; set; }
    public int DiemTichLuy { get; set; } = 0;
    public string? SoDienThoai { get; set; }
    public string HoTen { get; set; } = null!;
    public DateTime? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public string? QueQuan { get; set; }
    public virtual NguoiDung NguoiDung { get; set; } = null!;
    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
}