using BookingRoom.Constants;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Controllers
{
    // Tiền tố /api/admin
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = AppRoles.SuperAdmin)] 
    public class AdminSystemController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;

        public AdminSystemController(IPermissionRepository permissionRepo, IAuthRepository authRepo, IUserRepository userRepo)
        {
            _permissionRepo = permissionRepo;
            _authRepo = authRepo;
            _userRepo = userRepo;
        }

        //Quản lí tài khoản người dùng (User Management)

        //  POST /api/admin/users
        [HttpPost("users/create-account")]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserVM model)
        {
            try
            {
                var result = await _authRepo.RegisterAsync(model, model.RoleName);
                return result.IsSuccess ? Ok(new { Message = "Tạo người dùng thành công" }) : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                 return BadRequest(new
                 {
                     Message = "Đã xảy ra lỗi khi tạo người dùng.",
                     Details = ex.Message
                 });
            }
        }

        // GET /api/admin/users?keyword=abc&pageIndex=1&pageSize=10
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] string? keyword, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _userRepo.GetUsersAsync(keyword, pageIndex, pageSize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi lấy danh sách người dùng.",
                    Details = ex.Message
                });
            }
        }

        // DELETE /api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userRepo.SoftDeleteUserAsync(id);
                return result ? Ok(new { Message = "Đã xóa tài khoản (Xóa mềm)" }) : NotFound(new { Message = "Không tìm thấy tài khoản" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi xóa tài khoản.",
                    Details = ex.Message
                });
            }
        }

        //POST /api/admin/users/assign-roles
        [HttpPost("users/assign-roles")]
        public async Task<IActionResult> AssignRolesToUser([FromBody] AssignUserRoleVM model)
        {
            try
            {
                var result = await _permissionRepo.GanVaiTroChoNhanVienAsync(model);
                return result ? Ok(new { Message = "Gán vai trò thành công" }) : BadRequest(new { Message = "Không tìm thấy người dùng" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi gán vai trò.",
                    Details = ex.Message
                });
            }
        }

        // Quản lí vai trò và quyền (Role and Permission Management)
        // Chuẩn REST: POST /api/admin/roles
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleVM model)
        {
            try
            {
                var result = await _permissionRepo.TaoVaiTroMoiAsync(model);
                return result ? Ok(new { Message = "Tạo vai trò thành công" }) : BadRequest(new { Message = "Tên vai trò đã tồn tại" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi tạo vai trò.",
                    Details = ex.Message
                });
            }
        }

        // GET /api/admin/permissions
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            try
            {
                var data = await _permissionRepo.LayDanhSachQuyenDeVeUIAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi lấy danh sách quyền.",
                    Details = ex.Message
                });
            }
        }

        // POST /api/admin/roles/assign-permissions
        [HttpPost("roles/assign-permissions")]
        public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignRolePermissionVM model)
        {
            try
            {
                var result = await _permissionRepo.GanQuyenChoVaiTroAsync(model);
                return result ? Ok(new { Message = "Gán quyền thành công" }) : BadRequest(new { Message = "Không tìm thấy vai trò" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi khi gán quyền.",
                    Details = ex.Message
                });
            }
        }
        // Tùng yêu cầu thêm 
        // GET /api/admin/users/{id}/roles
        [HttpGet("users/{id}/roles")]
        public async Task<IActionResult> GetRolesOfUser(Guid id)
        {
            try
            {
                // Trả về thẳng 1 mảng các Guid: [ "id-role-1", "id-role-2" ]
                var roleIds = await _permissionRepo.LayDanhSachVaiTroCuaNhanVienAsync(id);
                return Ok(roleIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy vai trò của nhân viên.", Details = ex.Message });
            }
        }

        // GET /api/admin/roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                // Trả về mảng object ẩn danh: [ { id: "...", tenVaiTro: "Admin" } ]
                var roles = await _permissionRepo.LayDanhSachVaiTroAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách vai trò.", Details = ex.Message });
            }
        }

        // GET /api/admin/roles/{id}/permissions
        [HttpGet("roles/{id}/permissions")]
        public async Task<IActionResult> GetPermissionsOfRole(Guid id)
        {
            try
            {
                // Trả về thẳng 1 mảng các Guid: [ "id-quyen-xem", "id-quyen-sua" ]
                var permissionIds = await _permissionRepo.LayDanhSachQuyenCuaVaiTroAsync(id);
                return Ok(permissionIds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách quyền của vai trò.", Details = ex.Message });
            }
        }
    }
}