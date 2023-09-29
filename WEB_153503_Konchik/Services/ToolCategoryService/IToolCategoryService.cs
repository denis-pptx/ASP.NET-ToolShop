namespace WEB_153503_Konchik.Services.ToolCategoryService;

public interface IToolCategoryService
{
    /// <summary>
    /// Получение списка всех категорий
    /// </summary>
    /// <returns></returns>
    
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}
