using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.Domain.Models;

public class Cart
{
    /// <summary>
    /// Список объектов в корзине
    /// key - идентификатор объекта
    /// </summary>
    public Dictionary<int, CartItem> CartItems { get; set; } = new();

    /// <summary>
    /// Добавить объект в корзину
    /// </summary>
    /// <param name="dish">Добавляемый объект</param>
    public virtual void AddToCart(Tool tool)
    {
        if (CartItems.ContainsKey(tool.Id))
            CartItems[tool.Id].Quantity++;
        else
            CartItems[tool.Id] = new CartItem()
            {
                Tool = tool,
                Quantity = 1
            };
    }

    /// <summary>
    /// Удалить объект из корзины
    /// </summary>
    /// <param name="id"> id удаляемого объекта</param>
    public virtual void Remove(int id)
    {
        CartItems.Remove(id);
    }

    /// <summary>
    /// Очистить корзину
    /// </summary>
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }

    /// <summary>
    /// Количество объектов в корзине
    /// </summary>
    public int Count => 
        CartItems.Sum(item => item.Value.Quantity);

    /// <summary>
    /// Общее сумма корзины
    /// </summary>
    public double TotalPrice => 
        CartItems.Sum(item => item.Value.Tool.Price * item.Value.Quantity);
}