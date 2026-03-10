using System;

namespace LogiHUB.Shared.DTOs
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public int ShipmentCount { get; set; }

        public int InvoiceCount { get; set; }
    }
}