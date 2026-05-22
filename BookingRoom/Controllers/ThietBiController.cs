using BookingRoom.Constants;
using BookingRoom.Data;
using BookingRoom.Entities;
using BookingRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Controllers
{
    [Route("api/equipments")]
    [ApiController]
    public class ThietBiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThietBiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // QUẢN LÝ KHO 

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetInventory()
        {
            try
            {
                // Global Query Filter tự động loại bỏ các thiết bị đã xóa mềm
                var thietBis = await _context.ThietBis
                    .Select(t => new { t.Id, t.TenThietBi, t.TongSoLuong, t.Icon })
                    .ToListAsync();
                return Ok(thietBis);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách thiết bị.", Details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Equipment.Create)]
        public async Task<IActionResult> CreateInventory([FromBody] ThietBiVM request)
        {
            try
            {
                if (request.TongSoLuong < 0)
                    return BadRequest(new { Message = "Số lượng không được âm." });

                var tb = new ThietBi
                {
                    TenThietBi = request.TenThietBi,
                    TongSoLuong = request.TongSoLuong,
                    Icon = request.Icon
                };

                _context.ThietBis.Add(tb);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Thêm danh mục thiết bị thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi tạo thiết bị.", Details = ex.Message });
            }
        }

        [HttpPatch("{id}/add-quantity")]
        [Authorize(Policy = AppPermissions.Equipment.Edit)]
        public async Task<IActionResult> AddQuantity(Guid id, [FromQuery] int amount = 1)
        {
            try
            {
                var tb = await _context.ThietBis.FindAsync(id);
                if (tb == null) return NotFound(new { Message = "Không tìm thấy thiết bị." });

                tb.TongSoLuong += amount;
                if (tb.TongSoLuong < 0) tb.TongSoLuong = 0;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã cập nhật số lượng!", TongSoLuong = tb.TongSoLuong });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật số lượng.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPermissions.Equipment.Delete)]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            try
            {
                var tb = await _context.ThietBis.FindAsync(id);
                if (tb == null) return NotFound(new { Message = "Không tìm thấy thiết bị." });

                // Kiểm tra xem thiết bị này có đang được lắp trong phòng nào không
                var dangSuDung = await _context.ThietBiPhongs.AnyAsync(tbp => tbp.ThietBiId == id);
                if (dangSuDung)
                    return BadRequest(new { Message = "Không thể xóa vì thiết bị này đang được gắn trong phòng!" });

                _context.ThietBis.Remove(tb);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã xóa thiết bị khỏi kho!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi xóa thiết bị.", Details = ex.Message });
            }
        }

        // QUẢN LÝ THIẾT BỊ TRONG PHÒNG 
        [HttpGet("room-assignments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomsWithEquipment()
        {
            try
            {
                 var rooms = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .Include(p => p.ThietBiPhongs)
                    .ThenInclude(tbp => tbp.ThietBi)
                .Select(p => new
                {
                    RoomId = p.Id,
                    p.SoPhong,
                    TenLoaiPhong = p.LoaiPhong != null ? p.LoaiPhong.TenLoaiPhong : "N/A",
                    p.Tang,
                    p.TrangThai,
                    // Lọc những thiết bị phòng chưa bị xóa mềm
                    Equipments = p.ThietBiPhongs.Where(t => !t.IsDeleted).Select(t => new
                    {
                        AssignId = t.Id,
                        EquipmentId = t.ThietBiId,
                        TenThietBi = t.ThietBi.TenThietBi,
                        Icon = t.ThietBi.Icon,
                        t.TinhTrang
                    })
                }).ToListAsync();

                            return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi lấy danh sách phòng và thiết bị.", Details = ex.Message });
            }
        }

        // Phân bổ thiết bị từ kho vào phòng
        [HttpPost("room-assignments")]
        [Authorize(Policy = AppPermissions.Room.Edit)]
        public async Task<IActionResult> AssignEquipment([FromBody] AssignThietBiVM request)
        {
            try
            {
                var thietBi = await _context.ThietBis.FindAsync(request.ThietBiId);
                if (thietBi == null)
                    return NotFound(new { Message = "Không tìm thấy thiết bị trong kho." });

                if (thietBi.TongSoLuong <= 0)
                    return BadRequest(new { Message = "Thiết bị này đã hết hàng trong kho tổng!" });

                //  Trừ kho tổng
                thietBi.TongSoLuong -= 1;

                // Thêm vào phòng
                var tbp = new ThietBiPhong
                {
                    PhongId = request.PhongId,
                    ThietBiId = request.ThietBiId,
                    TinhTrang = request.TinhTrang
                };

                _context.ThietBiPhongs.Add(tbp);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Đã thêm thiết bị vào phòng!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi phân bổ thiết bị.", Details = ex.Message });
            }   
        }

        [HttpPut("room-assignments/{id}")]
        [Authorize(Policy = AppPermissions.Housekeeping.ReportDamage)]
        public async Task<IActionResult> UpdateEquipmentStatus(Guid id, [FromBody] UpdateTinhTrangVM request)
        {
            try
            {
                var tbp = await _context.ThietBiPhongs.FindAsync(id);
                if (tbp == null)
                    return NotFound(new { Message = "Không tìm thấy bản ghi phân bổ này." });

                tbp.TinhTrang = request.TinhTrang;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Cập nhật tình trạng thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi cập nhật tình trạng thiết bị.", Details = ex.Message });
            }
        }

        // Rút thiết bị khỏi phòng & Hoàn trả về kho
        [HttpDelete("room-assignments/{id}")]
        [Authorize(Policy = AppPermissions.Room.Edit)]
        public async Task<IActionResult> RemoveEquipment(Guid id)
        {
            try
            {
                var tbp = await _context.ThietBiPhongs.FindAsync(id);
                if (tbp == null) return NotFound(new { Message = "Không tìm thấy bản ghi phân bổ này." });

                var thietBi = await _context.ThietBis.FindAsync(tbp.ThietBiId);
                if (thietBi != null)
                {
                    thietBi.TongSoLuong += 1;
                }
                _context.ThietBiPhongs.Remove(tbp);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Đã gỡ thiết bị khỏi phòng và hoàn trả về kho!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đã xảy ra lỗi khi gỡ thiết bị khỏi phòng.", Details = ex.Message });
            }
        }
    }
}