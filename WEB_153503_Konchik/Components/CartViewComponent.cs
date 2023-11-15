using WEB_153503_Konchik.Extensions;

namespace WEB_153503_Konchik.Components;

public class CartViewComponent : ViewComponent
{
    private readonly Cart _cart;
    public CartViewComponent(Cart cart)
    {
        _cart = cart;
    }
    public IViewComponentResult Invoke()
    {
        return View(_cart);
    }
}
