namespace WEB_153503_Konchik.API.Data;

public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        // Получение контекста БД.
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Выполнение миграций.
        await context.Database.MigrateAsync();

        // Заполнение категорий.
        await context.Categories.AddRangeAsync(new List<Category>()
        {
            new() { Name = "Бетонные работы", NormalizedName = "concrete-works" },
            new() { Name = "Отделочные работы", NormalizedName = "finishing-works" },
            new() { Name = "Измерение", NormalizedName = "measurement" },
            new() { Name = "Сверление и крепеж", NormalizedName = "fasteners-and-installation" },
            new() { Name = "Защита и безопасность", NormalizedName = "protection-and-safety" }
        });
        await context.SaveChangesAsync();

        // Заполнение инструментов.
        string imageRoot = $"{app.Configuration["AppUrl"]!}/images";

        await context.Tools.AddRangeAsync(new List<Tool>()
        {
            new ()
            {
                Name = "Бетоносмеситель",
                Description = "Мощный бетоносмеситель для бетонных работ",
                Price = 450.0,
                Image = $"{imageRoot}/concrete-mixer.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("concrete-works"))
            },
            new ()
            {
                Name = "Буровая машина",
                Description = "Буровая машина для бетонных работ",
                Price = 320.0,
                Image = $"{imageRoot}/drilling-machine.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("concrete-works"))
            },
            new ()
            {
                Name = "Влагомер бетона",
                Description = "Универсальный влагомер для бетонных работ",
                Price = 48.0,
                Image = $"{imageRoot}/concrete-moisture-meter.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("concrete-works"))
            },
            new ()
            {
                Name = "Гладилка для бетона",
                Description = "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                Price = 80.0,
                Image = $"{imageRoot}/ironer-for-concrete.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("concrete-works"))
            },
            new ()
            {
                Name = "Стеклорез",
                Description = "Профессиональный стеклорез для отделочных работ",
                Price = 25.0,
                Image = $"{imageRoot}/glass-cutter.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("finishing-works"))
            },
            new ()
            {
                Name = "Шпатель для штукатурки",
                Description = "Шпатель для отделочных работ",
                Price = 15.0,
                Image = $"{imageRoot}/putty-knife.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("finishing-works"))
            },
            new ()
            {
                Name = "Лазерный дальномер",
                Description = "Высокоточный лазерный дальномер для измерений",
                Price = 120.0,
                Image = $"{imageRoot}/laser-measurer.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("measurement"))
            },
            new ()
            {
                Name = "Рулетка",
                Description = "Рулетка для измерений",
                Price = 12.0,
                Image = $"{imageRoot}/tape-measure.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("measurement"))
            },
            new ()
            {
                Name = "Гвоздезабиватель",
                Description = "Гвоздезабиватель для крепления материалов",
                Price = 60.0,
                Image = $"{imageRoot}/nail-gun.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new ()
            {
                Name = "Шуруповерт",
                Description = "Шуруповерт для монтажных работ",
                Price = 75.0,
                Image = $"{imageRoot}/screwdriver.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new ()
            {
                Name = "Маска для сварки",
                Description = "Защитная маска для сварки",
                Price = 35.0,
                Image = $"{imageRoot}/welding-mask.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("protection-and-safety"))
            },
            new ()
            {
                Name = "Перчатки",
                Description = "Защитные перчатки для работы с электричеством",
                Price = 18.0,
                Image = $"{imageRoot}/gloves.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("protection-and-safety"))
            },
            new ()
            {
                Name = "Дрель",
                Description = "Мощная дрель для сверления",
                Price = 80.0,
                Image = $"{imageRoot}/drill.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("fasteners-and-installation"))
            },
            new ()
            {
                Name = "Перфоратор",
                Description = "Перфоратор для сверления отверстий в бетоне",
                Price = 150.0,
                Image = $"{imageRoot}/hammer-drill.jpg",
                Category = await context.Categories.SingleAsync(c => c.NormalizedName.Equals("fasteners-and-installation"))
            }
        });
        
        await context.SaveChangesAsync();
    }
}
