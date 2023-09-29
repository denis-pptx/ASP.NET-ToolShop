namespace WEB_153503_Konchik.Domain.Models;

public class ListModel<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
