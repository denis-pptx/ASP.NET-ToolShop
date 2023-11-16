using WEB_153503_Konchik.Extensions;

namespace WEB_153503_Konchik.Controllers;


public class ProductController : Controller
{
    readonly IToolCategoryService _toolCategoryService;
    readonly IToolService _toolService;

    public ProductController(IToolCategoryService toolCategoryService, IToolService toolService)
    {
        _toolCategoryService = toolCategoryService;
        _toolService = toolService;
    }

    [Route("Catalog")]
    [Route("Catalog/{category?}")]
    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categoryResponse =  await _toolCategoryService.GetCategoryListAsync();
        if (!categoryResponse.Success)
            return NotFound(categoryResponse.ErrorMessage);

        ViewData["categories"] = categoryResponse.Data;
        ViewData["currentCategory"] = categoryResponse.Data?.SingleOrDefault(c => c.NormalizedName == category);

        var productResponse = await _toolService.GetToolListAsync(category, pageNo);
        if (!productResponse.Success)
            return NotFound(productResponse.ErrorMessage);

        if (Request.IsAjaxRequest())
        {
            ListModel<Tool> data = productResponse.Data!;
            return PartialView("_ProductIndexPartial", new
            {
                data.Items,
                data.CurrentPage,
                data.TotalPages,
                CategoryNormalizedName = category
            });
        }
        else
        {
            return View(productResponse.Data); 
        }
    }
}
