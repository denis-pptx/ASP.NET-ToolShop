namespace WEB_153503_Konchik.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IToolCategoryService _toolCategoryService;

    public CategoriesController(IToolCategoryService toolCategoryService)
    {
        _toolCategoryService = toolCategoryService;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<ResponseData<List<Tool>>>> GetCategories()
    {
        var result = await _toolCategoryService.GetCategoryListAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
