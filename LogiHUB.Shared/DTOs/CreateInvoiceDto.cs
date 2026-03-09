using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? ShipmentId { get; set; }
    }
}