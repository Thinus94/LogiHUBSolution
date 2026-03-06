using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LogiHUB.Shared.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [JsonIgnore] // Prevent circular reference
        public List<Shipment> Shipments { get; set; } = new();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
