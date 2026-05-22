using BookingRoom.ViewModels;

namespace BookingRoom.Services
{
    public interface IPermissionRepository
    {
        Task<bool> TaoVaiTroMoiAsync(CreateRoleVM model);
        Task<List<ChucNangTreeVM>> LayDanhSachQuyenDeVeUIAsync();
        Task<bool> GanQuyenChoVaiTroAsync(AssignRolePermissionVM model);
        Task<bool> GanVaiTroChoNhanVienAsync(AssignUserRoleVM model);
        Task<List<Guid>> LayDanhSachVaiTroCuaNhanVienAsync(Guid userId);
        Task<object> LayDanhSachVaiTroAsync();
        Task<List<Guid>> LayDanhSachQuyenCuaVaiTroAsync(Guid roleId);
    }
}
