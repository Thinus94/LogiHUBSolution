using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.Enums
{
    public enum InvoiceStatus
    {
        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "Paid")]
        Paid,

        [Display(Name = "Overdue")]
        Overdue,

        [Display(Name = "Cancelled")]
        Cancelled
    }
}