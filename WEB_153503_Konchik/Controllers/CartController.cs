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

    public async Task<IActionResult> Add(int id, string returnUrl)
    {
        var tool = await _toolService.GetToolByIdAsync(id);
        if (tool.Success)
        {
            _cart.AddToCart(tool.Data!);
        }

        return Redirect(returnUrl);
    }
}
