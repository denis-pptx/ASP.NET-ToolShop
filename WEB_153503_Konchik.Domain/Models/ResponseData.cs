namespace WEB_153503_Konchik.Domain.Models;

public class ResponseData<T>
{
    public T? Data { get; set; } = default;
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
}
