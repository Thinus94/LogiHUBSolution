using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShipmentNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Origin { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime PickupDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Range(0.01, 100000)]
        public decimal WeightKg { get; set; }

        [Required]
        [MaxLength(50)]
        public string CustomerCode { get; set; } = string.Empty;
    }
}