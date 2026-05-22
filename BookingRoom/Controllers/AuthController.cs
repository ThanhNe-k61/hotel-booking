using BookingRoom.Constants;
using BookingRoom.Services;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo) => _authRepo = authRepo;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            try
            {
                var result = await _authRepo.RegisterAsync(model, AppRoles.Customer);
                return result.IsSuccess ? Ok(new { Message = "Đăng ký thành công" }) : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi trong quá trình đăng ký",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            try
            {
                var result = await _authRepo.LoginAsync(model);
                return result.IsSuccess ? Ok(new { Token = result.Token }) : Unauthorized(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Đã xảy ra lỗi trong quá trình đăng nhập",
                    Details = ex.Message
                });
            }
        }
    }
}