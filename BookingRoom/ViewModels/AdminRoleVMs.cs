using System.ComponentModel.DataAnnotations;

namespace BookingRoom.ViewModels
{
    // Dùng để tạo Vai trò mới
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Tên vai trò không được để trống")]
        public string TenVaiTro { get; set; } = null!;
    }

    // Dùng để vẽ cây Checkbox trên giao diện
    public class ChucNangTreeVM
    {
        public Guid Id { get; set; }
        public string TenChucNang { get; set; } = null!;
        public List<QuyenItemVM> Quyens { get; set; } = new List<QuyenItemVM>();
    }

    public class QuyenItemVM
    {
        public Guid Id { get; set; }
        public string TenQuyen { get; set; } = null!;
        public string GiaTriQuyen { get; set; } = null!;
    }

    // Dùng khi sếp bấm "Lưu" để gán các Checkbox đã tick cho Vai trò
    public class AssignRolePermissionVM
    {
        [Required] public Guid VaiTroId { get; set; }
        [Required] public List<Guid> QuyenIds { get; set; } = new List<Guid>();
    }

    //  Dùng khi gán Vai trò cho Nhân viên cụ thể
    public class AssignUserRoleVM
    {
        [Required] public Guid NguoiDungId { get; set; }
        [Required] public List<Guid> VaiTroIds { get; set; } = new List<Guid>();
    }
}