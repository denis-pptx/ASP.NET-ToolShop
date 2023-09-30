using Microsoft.EntityFrameworkCore;

namespace WEB_153503_Konchik.API.Services;

public class ToolService : IToolService
{
    private const int _maxPageSize = 20;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public ToolService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ResponseData<ListModel<Tool>>> GetToolListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;

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
        var tool = await _context.Tools.FindAsync(id);
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
        var tool = await _context.Tools.FindAsync(id);
        if (tool is null)
        {
            return new ResponseData<string>
            {
                Success = false,
                ErrorMessage = "Tool was not found"
            };
        }

        string imageRoot = Path.Combine(_configuration["AppUrl"]!, "images");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
        
        string imagePath = Path.Combine(imageRoot, uniqueFileName);

        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }

        tool.Image = imagePath;
        await _context.SaveChangesAsync();

        return new ResponseData<string>
        {
            Data = tool.Image,
            Success = true
        };

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
        oldTool.Image = tool.Image;
        oldTool.Category = tool.Category;

        await _context.SaveChangesAsync();
    }
}
