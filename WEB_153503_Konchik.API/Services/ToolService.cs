using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WEB_153503_Konchik.API.Services;

public class ToolService : IToolService
{
    public int MaxPageSize { get; } = 20;

    private readonly AppDbContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToolService(AppDbContext context,
        IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;

        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseData<ListModel<Tool>>> GetToolListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > MaxPageSize)
            pageSize = MaxPageSize;

        var query = _context.Tools.AsQueryable();
        var dataList = new ListModel<Tool>();

        query = query.Where(d => categoryNormalizedName == null || 
                            d.Category!.NormalizedName.Equals(categoryNormalizedName))
                     .Include(t => t.Category);

        var count = query.Count();
        if (count == 0)
        {
            return new ResponseData<ListModel<Tool>>
            {
                Data = dataList
            };
        }

        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages)
        {
            return new ResponseData<ListModel<Tool>>
            {
                Data = null,
                Success = false,
                ErrorMessage = "No such page"
            };
        }
            
        dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;
        return new ResponseData<ListModel<Tool>>
        {
            Data = dataList
        };
    }

    public async Task<ResponseData<Tool>> CreateToolAsync(Tool tool)
    {
        _context.Tools.Add(tool);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new ResponseData<Tool> {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }

        return new ResponseData<Tool> 
        { 
            Data = tool 
        };
    }

    public async Task<ResponseData<Tool>> GetToolByIdAsync(int id)
    {
        var tool = await _context.Tools.Include(t => t.Category).SingleOrDefaultAsync(t => t.Id == id);
        if (tool is null)
        {
            return new()
            {
                Success = false,
                ErrorMessage = "Tool was not found"
            };
        }

        return new()
        {
            Data = tool
        };
    }

    public async Task DeleteToolAsync(int id)
    {
        var tool = await _context.Tools.FindAsync(id);
        if (tool is null)
        {
            throw new Exception("Tool was not found");
        }

        _context.Tools.Remove(tool);
        await _context.SaveChangesAsync();
    }


    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        var responseData = new ResponseData<string>();
        var tool = await _context.Tools.FindAsync(id);
        if (tool == null)
        {
            responseData.Success = false;
            responseData.ErrorMessage = "No item found";
            return responseData;
        }
        var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
        var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

        if (formFile != null)
        {
            if (!string.IsNullOrEmpty(tool.Image))
            {
                var prevImage = Path.GetFileName(tool.Image);
                var prevImagePath = Path.Combine(imageFolder, prevImage);
                if (File.Exists(prevImagePath))
                {
                    File.Delete(prevImagePath);
                }
            }
            var ext = Path.GetExtension(formFile.FileName);
            var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
            var filePath = Path.Combine(imageFolder, fName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            tool.Image = $"{host}/images/{fName}";
            await _context.SaveChangesAsync();
        }
        responseData.Data = tool.Image;
        return responseData;
    }

    public async Task UpdateToolAsync(int id, Tool tool)
    {
        var oldTool = await _context.Tools.FindAsync(id);
        if (oldTool is null)
        {
            throw new Exception("Tool was not found");
        }

        oldTool.Name = tool.Name;
        oldTool.Description = tool.Description;
        oldTool.Price = tool.Price;
        oldTool.CategoryId = tool.CategoryId;

        await _context.SaveChangesAsync();
    }
}
