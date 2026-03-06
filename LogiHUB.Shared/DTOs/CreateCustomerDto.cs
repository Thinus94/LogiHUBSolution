using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class CreateCustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
    }
}