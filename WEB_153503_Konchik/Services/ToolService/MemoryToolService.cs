namespace WEB_153503_Konchik.Services.ToolService;

public class MemoryToolService : IToolService
{
    List<Tool> _tools;
    List<Category> _categories;
    IConfiguration _configuration;

    public MemoryToolService(IConfiguration configuration, IToolCategoryService toolCategoryService)
    {
        _configuration = configuration;

        _categories = toolCategoryService.GetCategoryListAsync().Result.Data;
        SetupData();
    }

    /// <summary>
    /// Инициализация списков
    /// </summary>
    private void SetupData()
    {
        _tools = new List<Tool>()
        {
            new Tool
            {
                Id = 1,
                Name = "Бетоносмеситель",
                Description = "Мощный бетоносмеситель для бетонных работ",
                Price = 450.0,
                Image = "images/concrete-mixer.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("concrete-works"))
            },
            new Tool
            {
                Id = 2,
                Name = "Буровая машина",
                Description = "Буровая машина для бетонных работ",
                Price = 320.0,
                Image = "images/drilling-machine.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("concrete-works"))
            },
            new Tool
            {
                Id = 13,
                Name = "Влагомер бетона",
                Description = "Универсальный влагомер для бетонных работ",
                Price = 48.0,
                Image = "images/concrete-moisture-meter.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("concrete-works"))
            },
            new Tool
            {
                Id = 14,
                Name = "Гладилка для бетона",
                Description = "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                Price = 80.0,
                Image = "images/ironer-for-concrete.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("concrete-works"))
            },
            new Tool
            {
                Id = 3,
                Name = "Стеклорез",
                Description = "Профессиональный стеклорез для отделочных работ",
                Price = 25.0,
                Image = "images/glass-cutter.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("finishing-works"))
            },
            new Tool
            {
                Id = 4,
                Name = "Шпатель для штукатурки",
                Description = "Шпатель для отделочных работ",
                Price = 15.0,
                Image = "images/putty-knife.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("finishing-works"))
            },
            new Tool
            {
                Id = 5,
                Name = "Лазерный дальномер",
                Description = "Высокоточный лазерный дальномер для измерений",
                Price = 120.0,
                Image = "images/laser-measurer.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("measurement"))
            },
            new Tool
            {
                Id = 6,
                Name = "Рулетка",
                Description = "Рулетка для измерений",
                Price = 12.0,
                Image = "images/tape-measure.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("measurement"))
            },
            new Tool
            {
                Id = 7,
                Name = "Гвоздезабиватель",
                Description = "Гвоздезабиватель для крепления материалов",
                Price = 60.0,
                Image = "images/nail-gun.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new Tool
            {
                Id = 8,
                Name = "Шуруповерт",
                Description = "Шуруповерт для монтажных работ",
                Price = 75.0,
                Image = "images/screwdriver.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new Tool
            {
                Id = 9,
                Name = "Маска для сварки",
                Description = "Защитная маска для сварки",
                Price = 35.0,
                Image = "images/welding-mask.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("protection-and-safety"))
            },
            new Tool
            {
                Id = 10,
                Name = "Перчатки",
                Description = "Защитные перчатки для работы с электричеством",
                Price = 18.0,
                Image = "images/gloves.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("protection-and-safety"))
            },
            new Tool
            {
                Id = 11,
                Name = "Дрель",
                Description = "Мощная дрель для сверления",
                Price = 80.0,
                Image = "images/drill.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new Tool
            {
                Id = 12,
                Name = "Перфоратор",
                Description = "Перфоратор для сверления отверстий в бетоне",
                Price = 150.0,
                Image = "images/hammer-drill.jpg",
                Category = _categories.Single(c => c.NormalizedName.Equals("fasteners-and-installation"))
            }
        };
    }

    public Task<ResponseData<ListModel<Tool>>> GetToolListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var result = new ResponseData<ListModel<Tool>>();

        if (Int32.TryParse(_configuration["ItemsPerPage"], out int itemsPerPage)) {
            var tools = _tools.Where(t => categoryNormalizedName == null || t.Category!.NormalizedName.Equals(categoryNormalizedName)).ToList();
            result.Data = new()
            {
                Items = tools.Skip((pageNo - 1) * itemsPerPage).Take(itemsPerPage).ToList(),
                CurrentPage = pageNo,
                TotalPages = (int)Math.Ceiling((double)tools.Count / itemsPerPage)
            };
        }
        else
        {
            result.Success = false;
            result.ErrorMessage = "Invalid \"ItemsPerPage\" value";
        }
        return Task.FromResult(result);
    }
    
    public Task<ResponseData<Tool>> CreateToolAsync(Tool tool, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteToolAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Tool>> GetToolByIdAsync(int id)
    {
        throw new NotImplementedException();
    }



    public Task UpdateToolAsync(int id, Tool tool, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
