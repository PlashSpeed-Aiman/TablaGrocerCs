namespace TableGrocer.EFCore.Models;

public class GroceryItem
{
    public int? Id { get; set; }
    public string? ItemName { get; set; }
    public string? Quantity { get; set; }
    public decimal? Price { get; set; }
    public bool? IsDone { get; set; }
    public int GroceryRunId { get; set; }
    public GroceryRun? GroceryRun { get; set; }

}