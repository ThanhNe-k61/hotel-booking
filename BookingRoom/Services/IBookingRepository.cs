using BookingRoom.ViewModels;

namespace BookingRoom.Services
{
    public interface IBookingRepository
    {
        Task<List<AvailableRoomTypeVM>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);
        Task<Guid> CreateBookingAsync(CreateBookingVM request);
        Task<bool> CheckInAsync(Guid datPhongId,CheckInVM request);
        Task<bool> RequestCheckoutInspectionAsync(Guid phongId);
        Task<bool> SubmitHousekeepingInspectionAsync(HousekeepingInspectVM request, Guid trucBuongId);
        Task<FolioInvoiceVM> GetInvoiceDetailsAsync(Guid datPhongId);
        Task<Guid> SplitInvoiceByRoomAsync(SplitBillByRoomVM request);
        Task<bool> ConfirmPaymentAndCheckoutAsync(Guid datPhongId, string phuongThucTT, Guid leTanId, Guid? chiTietDatPhongId = null);
        Task<bool> AddServiceToRoomAsync(AddServiceUsageVM request);
        Task<bool> MarkRoomAsCleanAsync(Guid phongId);
    }
}