namespace TableGrocer.EFCore.Models;

public class TemplateItem
{
    public int? Id  { get; set; }
    public string? ItemName  { get; set; }
    public string? Quantity { get; set; } 
    public int? TemplateId { get; set; }
    public Template Template  { get; set; }
}