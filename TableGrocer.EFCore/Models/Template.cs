namespace TableGrocer.EFCore.Models;

public class Template
{
    public int? Id { get; set; }
    public string? TemplateName { get; set; }
    public List<TemplateItem>? TemplateItems { get; set; }
}