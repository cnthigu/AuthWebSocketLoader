namespace ServerWebSocket.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string HWID { get; set; } = null!;
    }
}
