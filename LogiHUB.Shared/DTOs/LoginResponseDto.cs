namespace LogiHUB.Shared.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public Guid ClientId { get; set; }

        public string CompanyName { get; set; } = string.Empty;
    }
}
