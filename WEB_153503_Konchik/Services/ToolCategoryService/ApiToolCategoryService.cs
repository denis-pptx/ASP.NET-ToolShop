using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace WEB_153503_Konchik.Services.ToolCategoryService;

public class ApiToolCategoryService : IToolCategoryService
{ 
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiToolCategoryService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiToolCategoryService(HttpClient httpClient, ILogger<ApiToolCategoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}Categories/");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<List<Category>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };

            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        return new ResponseData<List<Category>>()
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}"
        };
    }
}
