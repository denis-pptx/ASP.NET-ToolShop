namespace WEB_153503_Konchik.Services.ToolCategoryService;

public class MemoryToolCategoryService : IToolCategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = new List<Category>()
        {
            new() { Id = 1, Name = "Бетонные работы", NormalizedName = "concrete-works" },
            new() { Id = 2, Name = "Отделочные работы", NormalizedName = "finishing-works" },
            new() { Id = 3, Name = "Измерение", NormalizedName = "measurement" },
            new() { Id = 4, Name = "Сверление и крепеж", NormalizedName = "fasteners-and-installation" },
            new() { Id = 5, Name = "Защита и безопасность", NormalizedName = "protection-and-safety" }
        };

        var result = new ResponseData<List<Category>>();
        result.Data = categories;

        return Task.FromResult(result);
    }

}
