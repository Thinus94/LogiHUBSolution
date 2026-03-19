using LogiHUB.Shared.Enums;

namespace LogiHUB.Shared.DTOs
{
    public class InvoiceQueryDto
    {
        public Guid? CustomerId { get; set; }

        public Guid? ShipmentId { get; set; }

        public string? Search { get; set; }

        public InvoiceStatus? Status { get; set; }

        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
