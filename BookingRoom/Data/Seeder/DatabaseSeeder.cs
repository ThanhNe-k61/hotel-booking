using BookingRoom.Constants;
using BookingRoom.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Data.Seeder
{
    public static class DatabaseSeeder
    {
        public static async Task SeedRoleAndPermissionsAsync(ApplicationDbContext context)
        {
            var rolePermissions = new Dictionary<string, List<string>>
            {
                {
                    AppRoles.SuperAdmin,new List<string>
                    {
                        //function permissions  
                        AppPermissions.Function.Delete,
                        AppPermissions.Function.Edit,
                        AppPermissions.Function.Create,
                        AppPermissions.Function.View,
                        //user permissions
                        AppPermissions.User.Delete,
                        AppPermissions.User.Edit,
                        AppPermissions.User.Create,
                        AppPermissions.User.View,
                        //role permissions
                        AppPermissions.Role.Delete,
                        AppPermissions.Role.Edit,
                        AppPermissions.Role.Create,
                        AppPermissions.Role.View,
                        //room permissions
                        AppPermissions.Room.Delete,
                        AppPermissions.Room.Edit,
                        AppPermissions.Room.Create,
                        AppPermissions.Room.View,
                        //PricingPolicy permissions
                        AppPermissions.PricingPolicy.Delete,
                        AppPermissions.PricingPolicy.Edit,
                        AppPermissions.PricingPolicy.Create,
                        AppPermissions.PricingPolicy.View,
                        //RoomType permissions
                        AppPermissions.RoomType.Delete,
                        AppPermissions.RoomType.Edit,
                        AppPermissions.RoomType.Create,
                        AppPermissions.RoomType.View,
                        //Service permissions
                        AppPermissions.Service.Delete,
                        AppPermissions.Service.Edit,
                        AppPermissions.Service.Create,
                        AppPermissions.Service.View,
                        //Equipment permissions
                        AppPermissions.Equipment.Delete,
                        AppPermissions.Equipment.Edit,
                        AppPermissions.Equipment.Create,
                        AppPermissions.Equipment.View,
                        //booking permissions
                        AppPermissions.Booking.View,
                    }
                },
                {
                    AppRoles.Receptionist, new List<string>
                    {
                        AppPermissions.RoomStatus.View,
                        AppPermissions.RoomStatus.Edit,
                        AppPermissions.Booking.UnknownCreate,
                        AppPermissions.Booking.View,
                        AppPermissions.Booking.Edit,
                        AppPermissions.Booking.Delete,
                        AppPermissions.Bill.View,
                        AppPermissions.Bill.Create,
                        AppPermissions.Bill.AddService,
                        AppPermissions.Reception.CheckIn,
                        AppPermissions.Reception.CheckOut,
                        AppPermissions.Room.View,
                        AppPermissions.PricingPolicy.View,
                        AppPermissions.RoomType.View,
                        AppPermissions.Service.View,
                    }
                },
                {
                    AppRoles.Housekeepeer, new List<string>
                    {
                        AppPermissions.RoomStatus.View,
                        AppPermissions.RoomStatus.Edit,
                        AppPermissions.Bill.AddService,
                        AppPermissions.Reception.CheckIn,
                        AppPermissions.Reception.CheckOut,
                        AppPermissions.Room.View,
                        AppPermissions.PricingPolicy.View,
                        AppPermissions.RoomType.View,
                        AppPermissions.Service.View,
                        AppPermissions.Housekeeping.ReportDamage,
                    }
                },
                {
                    AppRoles.Customer,new List<string>
                    {
                        AppPermissions.Booking.Create,
                        AppPermissions.Booking.View,
                        AppPermissions.Booking.Edit,
                        AppPermissions.Booking.Delete,
                        AppPermissions.Bill.View,
                        AppPermissions.Service.View,
                        AppPermissions.Profile.View,
                        AppPermissions.Profile.Edit,
                        AppPermissions.Profile.BookingHistory,
                        AppPermissions.PricingPolicy.View,
                    }
                },
            };

            foreach (var roleItem in rolePermissions)
            {
                var roleName = roleItem.Key;
                var permissions = roleItem.Value;

                // Kiểm tra và tạo VaiTrò
                var role = await context.VaiTros.FirstOrDefaultAsync(r => r.TenVaiTro == roleName);
                if (role == null)
                {
                    role = new VaiTro { TenVaiTro = roleName };
                    context.VaiTros.Add(role);
                    await context.SaveChangesAsync(); // Lưu để có ID
                }

                // Xử lý các Quyền
                foreach (var permissionStr in permissions)
                {
                    // Tách chuỗi (VD: "Booking.Create" -> function="Booking", action="Create")
                    var parts = permissionStr.Split('.');
                    if (parts.Length != 2) continue;

                    var moduleName = parts[0];
                    var actionName = parts[1];

                    // Kiểm tra Chức Năng
                    var chucNang = await context.ChucNangs.FirstOrDefaultAsync(c => c.TenChucNang == moduleName);
                    if (chucNang == null)
                    {
                        chucNang = new ChucNang { TenChucNang = moduleName, MoTa = $"Quản lý {moduleName}" };
                        context.ChucNangs.Add(chucNang);
                        await context.SaveChangesAsync();
                    }

                    // Kiểm tra Quyền
                    var quyen = await context.Quyens.FirstOrDefaultAsync(q => q.GiaTriQuyen == permissionStr);
                    if (quyen == null)
                    {
                        quyen = new Quyen
                        {
                            ChucNangId = chucNang.Id,
                            TenQuyen = actionName,
                            GiaTriQuyen = permissionStr
                        };
                        context.Quyens.Add(quyen);
                        await context.SaveChangesAsync();
                    }

                    //  Mapping VaiTro - Quyen (Bảng VaiTroQuyen)
                    var existsMap = await context.VaiTroQuyens
                        .AnyAsync(vq => vq.VaiTroId == role.Id && vq.QuyenId == quyen.Id);
                    if (!existsMap)
                    {
                        context.VaiTroQuyens.Add(new VaiTroQuyen { VaiTroId = role.Id, QuyenId = quyen.Id });
                    }
                }
            }
            var adminUser = await context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == "admin@booking.com");

            if (adminUser == null)
            {
                // Nếu chưa có thì tạo mới hoàn toàn
                adminUser = new NguoiDung
                {
                    TenDangNhap = "superadmin",
                    HoTen = "Quản Trị Hệ Thống",
                    Email = "admin@booking.com",
                    MatKhauHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    DefaultRole = UserRoleEnum.SuperAdmin,
                    IsProfileCompleted = true
                };
                context.NguoiDungs.Add(adminUser);
                await context.SaveChangesAsync();

                var superAdminRole = await context.VaiTros.FirstOrDefaultAsync(r => r.TenVaiTro == AppRoles.SuperAdmin);
                if (superAdminRole != null)
                {
                    context.NguoiDungVaiTros.Add(new NguoiDungVaiTro
                    {
                        NguoiDungId = adminUser.Id,
                        VaiTroId = superAdminRole.Id
                    });
                }
            }
            else
            {
                // Nếu ĐÃ CÓ rồi -> Ép cập nhật lại Role và Cờ hoàn thành
                adminUser.DefaultRole = UserRoleEnum.SuperAdmin;
                adminUser.IsProfileCompleted = true;
            }

            await context.SaveChangesAsync();
        }
    }
}