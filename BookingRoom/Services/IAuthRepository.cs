using BookingRoom.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BookingRoom.Services
{
    public interface IAuthRepository
    {
        Task<AuthResultVM> RegisterAsync(RegisterVM model, string roleName);
        Task<AuthResultVM> LoginAsync(LoginVM model);
        SecurityToken GenerateJwtToken(List<Claim> authClaims);
    }
}
