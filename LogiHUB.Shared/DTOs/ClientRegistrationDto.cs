using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class ClientRegistrationDto
    {
        public string CompanyName { get; set; } = string.Empty;

        public string ContactName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}