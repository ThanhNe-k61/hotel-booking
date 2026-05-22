using System.ComponentModel.DataAnnotations;

namespace BookingRoom.ViewModels
{
    public class RegisterVM
    {
        [Required] public string TenDangNhap { get; set; } = null!;
        [Required] public string HoTen { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required, MinLength(6)] public string Password { get; set; } = null!;
    }
}
