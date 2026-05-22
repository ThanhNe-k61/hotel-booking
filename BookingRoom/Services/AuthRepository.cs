using BCrypt.Net;
using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingRoom.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResultVM> RegisterAsync(RegisterVM model, string roleName)
        {
            try
            {
                if (await _context.NguoiDungs.AnyAsync(u => u.Email == model.Email || u.TenDangNhap == model.TenDangNhap))
                    return new AuthResultVM { IsSuccess = false, Errors = new[] { "Email hoặc Tên đăng nhập đã tồn tại" } };
                //Map roleName sang UserRoleEnum để sau này có thể dùng chung cho nhiều mục đích khác
                UserRoleEnum defaultRole = roleName switch
                {
                    AppRoles.SuperAdmin => UserRoleEnum.SuperAdmin,
                    AppRoles.Receptionist => UserRoleEnum.Receptionist,
                    AppRoles.Housekeepeer => UserRoleEnum.Housekeeper,
                    _ => UserRoleEnum.Customer
                };

                var newUser = new NguoiDung
                {
                    TenDangNhap = model.TenDangNhap,
                    HoTen = model.HoTen,
                    Email = model.Email,
                    MatKhauHash = BCrypt.Net.BCrypt.HashPassword(model.Password), // Hash bằng BCrypt
                    DefaultRole = defaultRole,
                    IsProfileCompleted = false
                };
                _context.NguoiDungs.Add(newUser);

                var role = await _context.VaiTros.FirstOrDefaultAsync(r => r.TenVaiTro == roleName);
                if (role != null)
                {
                    _context.NguoiDungVaiTros.Add(new NguoiDungVaiTro { NguoiDungId = newUser.Id, VaiTroId = role.Id });
                }
                else return new AuthResultVM { IsSuccess = false, Errors = new[] { "Role không hợp lệ" } };

                await _context.SaveChangesAsync();
                return new AuthResultVM { IsSuccess = true };
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return new AuthResultVM { IsSuccess = false, Errors = new[] { "Đăng ký thất bại: " + ex.Message } };
            }

        }
        public async Task<AuthResultVM> LoginAsync(LoginVM model)
        {
            try
            {
                var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.MatKhauHash))
                    return new AuthResultVM { IsSuccess = false, Errors = new[] { "Sai tài khoản hoặc mật khẩu" } };

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email!)
                };

                string roleId = "";
                switch (user.DefaultRole)
                {
                    case UserRoleEnum.Customer:
                        roleId = await _context.KhachHangs
                            .Where(k => k.NguoiDungId == user.Id)
                            .Select(k => k.Id.ToString())
                            .FirstOrDefaultAsync() ?? "";
                        break;
                    case UserRoleEnum.Receptionist:
                        roleId = await _context.LeTans
                            .Where(l => l.NguoiDungId == user.Id)
                            .Select(l => l.Id.ToString())
                            .FirstOrDefaultAsync() ?? "";
                        break;
                    case UserRoleEnum.Housekeeper:
                        roleId = await _context.TrucBuongs
                            .Where(t => t.NguoiDungId == user.Id)
                            .Select(t => t.Id.ToString())
                            .FirstOrDefaultAsync() ?? "";
                        break;
                    case UserRoleEnum.SuperAdmin:
                        break;
                }

                // Nhét RoleId vào Token
                if (!string.IsNullOrEmpty(roleId))
                {
                    authClaims.Add(new Claim("RoleId", roleId));
                }
                if (!user.IsProfileCompleted)
                {
                    // cấp quyền tạm thời để nhập thông tin hồ sơ (Frontend thấy có quyền này thì chuyển sang trang điền thông tin)
                    authClaims.Add(new Claim("Permission", "System.Onboarding"));
                    var limitedToken = GenerateJwtToken(authClaims);

                    return new AuthResultVM
                    {
                        IsSuccess = true,
                        Token = new JwtSecurityTokenHandler().WriteToken(limitedToken),
                        RequireProfileUpdate = true // Frontend thấy True thì chuyển sang trang điền thông tin
                    };
                }
                // NẾU ĐÃ CÓ HỒ SƠ: Nạp full quyền bình thường
                var roleIds = await _context.NguoiDungVaiTros
                    .Where(nv => nv.NguoiDungId == user.Id).
                    Select(nv => nv.VaiTroId).
                    ToListAsync();

                var roleNames = await _context.VaiTros
                        .Where(vt => roleIds.Contains(vt.Id)).
                        Select(vt => vt.TenVaiTro).
                        ToListAsync();
                foreach (var role in roleNames) authClaims.Add(new Claim(ClaimTypes.Role, role));

                var permissions = await _context.VaiTroQuyens
                    .Where(vq => roleIds.Contains(vq.VaiTroId)).
                    Select(vq => vq.Quyen.GiaTriQuyen).
                    Distinct().
                    ToListAsync();

                foreach (var permission in permissions) authClaims.Add(new Claim("Permission", permission));

                var token = GenerateJwtToken(authClaims);
                return new AuthResultVM { IsSuccess = true, Token = new JwtSecurityTokenHandler().WriteToken(token), RequireProfileUpdate = false };
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return new AuthResultVM { IsSuccess = false, Errors = new[] { "Đăng nhập thất bại: " + ex.Message } };
            }
        }
        private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            return new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );
        }

        SecurityToken IAuthRepository.GenerateJwtToken(List<Claim> authClaims)
        {
            return GenerateJwtToken(authClaims);
        }
    }
}

