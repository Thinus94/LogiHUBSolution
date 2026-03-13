using LogiHUB.Shared.Enums;

namespace LogiHUB.Shared.DTOs
{
    public class ShipmentResponseDto
    {
        public Guid Id { get; set; }

        public string ShipmentNumber { get; set; } = string.Empty;

        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public ShipmentStatus Status { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal WeightKg { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int InvoiceCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}