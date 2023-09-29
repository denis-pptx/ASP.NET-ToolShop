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

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categoryResponse =  await _toolCategoryService.GetCategoryListAsync();
        if (!categoryResponse.Success)
            return NotFound(categoryResponse.ErrorMessage);

        ViewData["caregories"] = categoryResponse.Data;
        ViewData["currentCategory"] = categoryResponse.Data.SingleOrDefault(c => c.NormalizedName == category);

        var productResponce = await _toolService.GetToolsListAsync(category, pageNo);
        if (!productResponce.Success)
            return NotFound(productResponce.ErrorMessage);

        return View(productResponce.Data);
    }
}
