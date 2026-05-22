using System.ComponentModel.DataAnnotations;

namespace BookingRoom.ViewModels
{
    public class AdminCreateUserVM : RegisterVM
    {
        [Required] public string RoleName { get; set; } = null!;
    }
}
