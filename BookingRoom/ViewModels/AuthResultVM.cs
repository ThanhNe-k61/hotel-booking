namespace BookingRoom.ViewModels
{
    public class AuthResultVM
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public bool  RequireProfileUpdate { get; set; } = false;

    }
}
