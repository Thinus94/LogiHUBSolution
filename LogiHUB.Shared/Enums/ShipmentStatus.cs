using System.ComponentModel.DataAnnotations;

namespace LogiHUB.Shared.Enums
{
    public enum ShipmentStatus
    {
        [Display(Name = "Created")]
        Created,

        [Display(Name = "In Transit")]
        InTransit,

        [Display(Name = "Delivered")]
        Delivered,

        [Display(Name = "Delayed")]
        Delayed,

        [Display(Name = "Cancelled")]
        Cancelled
    }
}