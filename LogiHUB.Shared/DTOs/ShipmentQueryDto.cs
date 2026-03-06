public class ShipmentQueryDto
{
    public string? Search { get; set; }

    public string? Status { get; set; }

    public Guid? CustomerId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}