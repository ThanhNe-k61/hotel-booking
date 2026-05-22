using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoom.Migrations
{
    public partial class AddTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. Trigger cho Đặt phòng: Tự động Đã trả phòng khi tất cả các phòng con đã có ThoiGianTraPhongThucTe
                CREATE OR REPLACE FUNCTION check_all_rooms_checked_out()
                RETURNS TRIGGER AS $$
                DECLARE
                    uncompleted_count INT;
                BEGIN
                    SELECT COUNT(*) INTO uncompleted_count
                    FROM ""ChiTietDatPhongs""
                    WHERE ""DatPhongId"" = NEW.""DatPhongId""
                      AND ""ThoiGianTraPhongThucTe"" IS NULL;

                    IF uncompleted_count = 0 THEN
                        UPDATE ""DatPhongs""
                        SET ""TrangThai"" = 'Đã trả phòng'
                        WHERE ""Id"" = NEW.""DatPhongId"" AND ""TrangThai"" != 'Đã trả phòng';
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER trg_auto_checkout_datphong
                AFTER UPDATE OF ""ThoiGianTraPhongThucTe"" ON ""ChiTietDatPhongs""
                FOR EACH ROW
                WHEN (NEW.""ThoiGianTraPhongThucTe"" IS NOT NULL)
                EXECUTE FUNCTION check_all_rooms_checked_out();

                -- 2. Trigger cho Hóa đơn: Tự động Đã thanh toán khi tất cả chi tiết hóa đơn con đã Đã thanh toán
                CREATE OR REPLACE FUNCTION check_all_bills_paid()
                RETURNS TRIGGER AS $$
                DECLARE
                    unpaid_count INT;
                BEGIN
                    SELECT COUNT(*) INTO unpaid_count
                    FROM ""ChiTietHoaDons""
                    WHERE ""HoaDonId"" = NEW.""HoaDonId""
                      AND ""TrangThaiThanhToan"" != 'Đã thanh toán';

                    IF unpaid_count = 0 THEN
                        UPDATE ""HoaDons""
                        SET ""TrangThaiThanhToan"" = 'Đã thanh toán'
                        WHERE ""Id"" = NEW.""HoaDonId"" AND ""TrangThaiThanhToan"" != 'Đã thanh toán';
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER trg_auto_paid_hoadon
                AFTER UPDATE OF ""TrangThaiThanhToan"" ON ""ChiTietHoaDons""
                FOR EACH ROW
                WHEN (NEW.""TrangThaiThanhToan"" = 'Đã thanh toán')
                EXECUTE FUNCTION check_all_bills_paid();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS trg_auto_checkout_datphong ON ""ChiTietDatPhongs"";
                DROP FUNCTION IF EXISTS check_all_rooms_checked_out();

                DROP TRIGGER IF EXISTS trg_auto_paid_hoadon ON ""ChiTietHoaDons"";
                DROP FUNCTION IF EXISTS check_all_bills_paid();
            ");
        }
    }
}