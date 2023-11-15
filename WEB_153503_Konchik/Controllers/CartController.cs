using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WEB_153503_Konchik.Extensions;

namespace WEB_153503_Konchik.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IToolService _toolService;
    public CartController(IToolService toolService)
    {
        _toolService = toolService;
    }

    public async Task<IActionResult> Add(int id, string returnUrl)
    {
        var tool = await _toolService.GetToolByIdAsync(id);
        if (tool.Success)
        {
            Cart cart = HttpContext.Session.Get<Cart>(nameof(Cart)) ?? new();
            cart.AddToCart(tool.Data!);

            HttpContext.Session.Set(nameof(Cart), cart);
        }

        return Redirect(returnUrl);
    }
}
