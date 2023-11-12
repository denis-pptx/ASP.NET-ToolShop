using Microsoft.AspNetCore.Authentication;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace WEB_153503_Konchik.Services.ToolService;

public class ApiToolService : IToolService
{
    private readonly HttpClient _httpClient;
    private string _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiToolService> _logger;
    private readonly HttpContext _httpContext;

    public ApiToolService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiToolService> logger, 
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value!;

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;

        _httpContext = httpContextAccessor.HttpContext!;
        var token = _httpContext.GetTokenAsync("access_token").Result;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }

    public async Task<ResponseData<ListModel<Tool>>> GetToolListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Tools/");
      
        if (categoryNormalizedName != null)
        {
            urlString.Append($"{categoryNormalizedName}/");
        };

        if (pageNo > 1)
        {
            urlString.Append($"{pageNo}");
        };

        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
        }

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Tool>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<ListModel<Tool>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        return new ResponseData<ListModel<Tool>>()
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
        };
    }


    public async Task<ResponseData<Tool>> CreateToolAsync(Tool tool, IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Tools");
        var response = await _httpClient.PostAsJsonAsync(uri, tool, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Tool>>(_serializerOptions);
            if (formFile != null)
            {
                await SaveImageAsync(data!.Data!.Id, formFile);
            }
            return data!;
        }
        _logger.LogError($"-----> object not created. Error: {response.StatusCode}");

        return new ResponseData<Tool>
        {
            Success = false,
            ErrorMessage = $"Объект не добавлен. Error: {response.StatusCode}"
        };
    }

    public async Task DeleteToolAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress!.AbsoluteUri}Tools/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        }
    }

    public async Task<ResponseData<Tool>> GetToolByIdAsync(int id)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Tools/tool{id}");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Tool>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<Tool>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        return new ResponseData<Tool>()
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
        };
    }

    
    public async Task UpdateToolAsync(int id, Tool tool, IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Tools/" + id);
        var response = await _httpClient.PutAsJsonAsync(uri, tool, _serializerOptions);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        }
        else if (formFile != null) 
        {
            int toolId = (await response.Content.ReadFromJsonAsync<ResponseData<Tool>>(_serializerOptions))!.Data!.Id;
            await SaveImageAsync(toolId, formFile);
        }
    }

    private async Task SaveImageAsync(int id, IFormFile image)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_httpClient.BaseAddress?.AbsoluteUri}Tools/{id}")
        };
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image.OpenReadStream());
        content.Add(streamContent, "formFile", image.FileName);
        request.Content = content;
        await _httpClient.SendAsync(request);
    }


}
