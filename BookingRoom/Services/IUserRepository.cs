using BookingRoom.ViewModels;

namespace BookingRoom.Services
{
    public interface IUserRepository
    {
        // Thêm tham số tìm kiếm (search) tìm nhân viên
        Task<PagedResultVM<object>> GetUsersAsync(string? keyword, int pageIndex = 1, int pageSize = 10);
        Task<bool> SoftDeleteUserAsync(Guid userId);
    }
}