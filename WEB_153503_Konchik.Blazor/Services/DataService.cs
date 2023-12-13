﻿using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WEB_153503_Konchik.Domain.Entities;
using WEB_153503_Konchik.Domain.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace WEB_153503_Konchik.Blazor.Services;

public class DataService : IDataService
{
    public event Action DataChanged;
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly int _pageSize = 3;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<DataService> _logger;

    public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider,
        ILogger<DataService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("PageSize").Get<int>();
        _accessTokenProvider = accessTokenProvider;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;

        ConfigureToken();
    }

    private async void ConfigureToken()
    {
        var tokenRequest = await _accessTokenProvider.RequestAccessToken();
        if (tokenRequest.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
    }

    public List<Category>? Categories { get; set; } = new();
    public List<Tool>? ToolList { get; set; } = new();
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }

    public async Task GetToolListAsync(string? categoryNormalizedName, int pageNo = 1)
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
                var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Tool>>>(_serializerOptions);
                ToolList = responseData?.Data?.Items;
                TotalPages = responseData?.Data?.TotalPages ?? 0;
                CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                DataChanged?.Invoke();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                Success = false;
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");

        Success = false;
        ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
    }

    public async Task<Tool?> GetToolByIdAsync(int id)
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Tools/tool{id}");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return (await response.Content.ReadFromJsonAsync<ResponseData<Tool>>(_serializerOptions)).Data;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");

                Success = false;
                ErrorMessage = $"Ошибка: {ex.Message}";
                return null;
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
        Success = false;
        ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";

        return null;
    }

    public async Task GetCategoryListAsync()
    {
        var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}Categories/");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var responseData = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                Categories = responseData?.Data;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                Success = false;
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        Success = false;
        ErrorMessage = $"Данные не получены от сервера. Error:{response.StatusCode}";
    }
}
