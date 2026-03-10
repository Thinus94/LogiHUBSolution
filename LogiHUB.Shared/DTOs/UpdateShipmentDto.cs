// LogiHUB.Shared/DTOs/UpdateShipmentDto.cs
using LogiHUB.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class UpdateShipmentDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ShipmentNumber { get; set; } = string.Empty;

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

        [Required]
        public ShipmentStatus Status { get; set; } 
    }
}