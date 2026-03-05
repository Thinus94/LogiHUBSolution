// LogiHUB.Shared/DTOs/CreateShipmentDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class CreateShipmentDto
    {
        [Required]
        public string Origin { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime PickupDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public decimal WeightKg { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}