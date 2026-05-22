public class CompleteProfileVM
{
    // Cập nhật vào NguoiDung
    public string SoDienThoai { get; set; } = null!;
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; } = null!;
    public string QueQuan { get; set; } = null!;

    // Cập nhật riêng cho Khách Hàng (Lễ tân/Trực buồng có thể gửi lên Null)
    public string? CCCD_Passport { get; set; }
    public string? QuocTich { get; set; }
}