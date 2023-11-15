using WEB_153503_Konchik.Extensions;

namespace WEB_153503_Konchik.Components;

public class CartViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _contextAccessor;
    public CartViewComponent(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public IViewComponentResult Invoke()
    {
        Cart? cart = _contextAccessor.HttpContext!.Session.Get<Cart>(nameof(Cart));
        return View(cart);
    }
}
