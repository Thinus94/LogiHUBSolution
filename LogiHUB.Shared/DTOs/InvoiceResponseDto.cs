using LogiHUB.Shared.Enums;

namespace LogiHUB.Shared.DTOs
{
    public class InvoiceResponseDto
    {
        public Guid Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public InvoiceStatus Status { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public Guid? ShipmentId { get; set; }

        public string? ShipmentNumber { get; set; }
    }
}