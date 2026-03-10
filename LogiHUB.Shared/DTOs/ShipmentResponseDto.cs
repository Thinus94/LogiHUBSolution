using System;

namespace LogiHUB.Shared.DTOs
{
    public class ShipmentResponseDto
    {
        public Guid Id { get; set; }
        public string ShipmentNumber { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal WeightKg { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid? InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
    }
}