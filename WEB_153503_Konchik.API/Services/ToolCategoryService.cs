namespace WEB_153503_Konchik.API.Services;

public class ToolCategoryService : IToolCategoryService
{
    private readonly AppDbContext _dbContext;

    public ToolCategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        return new ResponseData<List<Category>>
        {
            Data = await _dbContext.Categories.ToListAsync()
        };
    }
}
