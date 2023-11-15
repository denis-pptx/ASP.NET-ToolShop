using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WEB_153503_Konchik.Extensions;

namespace WEB_153503_Konchik.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IToolService _toolService;
    private readonly Cart _cart;

    public CartController(IToolService toolService, Cart cart)
    {
        _toolService = toolService;
        _cart = cart;
    }

    public IActionResult Index()
    {
        return View(_cart.CartItems);
    }
    public async Task<IActionResult> Add(int id, string returnUrl)
    {
        var response = await _toolService.GetToolByIdAsync(id);
        if (response.Success)
        {
            _cart.AddToCart(response.Data!);
        }

        return Redirect(returnUrl);
    }

    public IActionResult Remove(int id, string returnUrl)
    {
        _cart.Remove(id);

        return Redirect(returnUrl);
    }
}
