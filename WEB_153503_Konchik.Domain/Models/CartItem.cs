using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.Domain.Models;

public class CartItem
{
    public Tool Tool { get; set; } = null!;
    public int Quantity { get; set; }
}
