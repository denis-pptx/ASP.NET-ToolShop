namespace WEB_153503_Konchik.Domain.Entities;

public class Tool
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? Image { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}
