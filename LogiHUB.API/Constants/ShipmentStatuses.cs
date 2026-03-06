namespace LogiHUB.API.Constants
{
    public static class ShipmentStatuses
    {
        public const string Created = "Created";
        public const string InTransit = "In Transit";
        public const string Delivered = "Delivered";
        public const string Delayed = "Delayed";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All =
        {
            Created,
            InTransit,
            Delivered,
            Delayed,
            Cancelled
        };
    }
}