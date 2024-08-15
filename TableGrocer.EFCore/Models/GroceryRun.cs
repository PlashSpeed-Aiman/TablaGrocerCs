namespace TableGrocer.EFCore.Models;

public class GroceryRun
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
    public string? PlaceOfPurchase { get; set; }
    public bool? IsDone { get; set; }
    public List<GroceryItem>? GroceryItems { get; set; }

}