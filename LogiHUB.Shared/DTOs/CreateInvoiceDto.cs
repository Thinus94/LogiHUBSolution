using LogiHUB.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public decimal Amount { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? ShipmentId { get; set; }
    }
}