namespace BookingRoom.ViewModels
{
    public class FolioInvoiceVM
    {
        public Guid DatPhongId { get; set; }
        public Guid HoaDonId { get; set; }
        public decimal TienPhong { get; set; }
        public decimal TienDichVu { get; set; }
        public decimal TienDenBu { get; set; }
        public decimal TienDaCoc { get; set; }
        public decimal TongPhaiThanhToan => (TienPhong + TienDichVu + TienDenBu) - TienDaCoc;
    }

    public class SplitBillByRoomVM
    {
        public Guid DatPhongId { get; set; } // Mã booking tổng
        public Guid ChiTietDatPhongId { get; set; } // ID phòng muốn thanh toán riêng
        public Guid LeTanId { get; set; } // Lễ tân thực hiện
    }
}
