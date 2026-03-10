using LogiHUB.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class UpdateInvoiceDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public InvoiceStatus Status { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? ShipmentId { get; set; }
    }
}