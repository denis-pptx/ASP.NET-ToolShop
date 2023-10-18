namespace WEB_153503_Konchik.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Tool> Tools { get; set; }
    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();

    }
}