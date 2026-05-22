namespace BookingRoom.Entities
{
    public class NguoiDung : BaseEntity
    {
        public string TenDangNhap { get; set; } = null!;
        public string MatKhauHash { get; set; } = null!;
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public string HoTen { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
        public string TrangThai { get; set; } = "Hoạt động";
        // Mặc định khi mới tạo tài khoản sẽ là false (Chưa có hồ sơ)
        public bool IsProfileCompleted { get; set; } = false;
        //  Vai trò mặc định bằng Enum (Tăng tốc độ kiểm tra)
        public UserRoleEnum DefaultRole { get; set; } = UserRoleEnum.Customer;

        public virtual ICollection<NguoiDungVaiTro> NguoiDungVaiTros { get; set; } = new List<NguoiDungVaiTro>();
        public virtual LeTan? LeTan { get; set; }
        public virtual TrucBuong? TrucBuong { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
    }
}