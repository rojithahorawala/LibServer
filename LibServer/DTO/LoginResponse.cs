namespace LibServer.DTO
{
    public class LoginResponse
    {
        public required bool Success { get; set; }
        public required string Message { get; set; }
        public required string Token { get; set; }
    }
}
