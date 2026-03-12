using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class ClientLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}