using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153503_Konchik.API.Data;
using WEB_153503_Konchik.API.Services;
using WEB_153503_Konchik.Domain.Entities;
using WEB_153503_Konchik.Domain.Models;

namespace WEB_153503_Konchik.Tests;

public class ToolServiceTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;

    #region ConstructorAndDispose
    public ToolServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new AppDbContext(_contextOptions);

        context.Database.EnsureCreated();

        context.Categories.AddRange(
            new() { Id = 1, Name = "Бетонные работы", NormalizedName = "concrete-works" },
            new() { Id = 2, Name = "Отделочные работы", NormalizedName = "finishing-works" });

        context.Tools.AddRange(
            new() { Id = 1, Name = "Бетоносмеситель", Price = 450.0, CategoryId = 1 },
            new() { Id = 2, Name = "Буровая машина", Price = 320.0, CategoryId = 1 },
            new() { Id = 3, Name = "Влагомер бетона", Price = 48.0, CategoryId = 1 },
            new() { Id = 4, Name = "Гладилка для бетона", Price = 80.0,  CategoryId = 1},

            new() { Id = 5, Name = "Стеклорез", Price = 25.0, CategoryId = 2 },
            new() { Id = 6, Name = "Шпатель для штукатурки", Price = 15.0, CategoryId = 2 },
            new() { Id = 7, Name = "Лазерный дальномер", Price = 120.0, CategoryId = 2 });

        context.SaveChanges();
    }

    AppDbContext CreateContext() => new AppDbContext(_contextOptions);
    public void Dispose() => _connection.Dispose();
    #endregion

    [Fact]
    public void ServiceReturnsFirstPageOfThreeItems()
    {
        // Arrange.
        using var context = CreateContext();
        var service = new ToolService(context, null!, null!);

        // Act.
        var result = service.GetToolListAsync(null).Result;

        // Assert.
        Assert.IsType<ResponseData<ListModel<Tool>>>(result);
        Assert.True(result.Success);
        Assert.Equal(1, result.Data!.CurrentPage);
        Assert.Equal(3, result.Data!.Items.Count);
        Assert.Equal(3, result.Data!.TotalPages);
        Assert.Equal(context.Tools.First(), result.Data.Items[0]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ServiceChoosesRightSpecifiedPage(int pageNo)
    {
        // Arrange.
        using var context = CreateContext();
        var service = new ToolService(context, null!, null!);

        // Act.
        var result = service.GetToolListAsync(null, pageNo).Result;

        // Assert.
        Assert.IsType<ResponseData<ListModel<Tool>>>(result);
        Assert.True(result.Success);
        Assert.Equal(pageNo, result.Data!.CurrentPage);
        Assert.Equal(3, result.Data!.TotalPages);
        Assert.Equal(context.Tools.Skip((pageNo - 1) * 3).First(), result.Data.Items[0]);
    }

    [Fact]
    public void ServicePerformsCorrectFilteringByCategory()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ToolService(context, null!, null!);

        // Act
        var result = service.GetToolListAsync("concrete-works").Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Tool>>>(result);
        Assert.True(result.Success);
        Assert.Equal(1, result.Data!.CurrentPage);
        Assert.Equal(3, result.Data.Items.Count);
        Assert.Equal(2, result.Data.TotalPages);
        Assert.All(result.Data.Items, item => Assert.True(item.CategoryId == 1));
    }


    [Fact]
    public void ServiceDoesNotAllowToSetPageSizeMoreThanMax()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ToolService(context, null!, null!);

        // Act
        var result = service.GetToolListAsync(null, 1, service.MaxPageSize + 1).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Tool>>>(result);
        Assert.True(result.Data!.Items.Count <= service.MaxPageSize);
    }

    [Fact]
    public void ServiceDoesNotAllowToSetPageNumberMoreThanPagesCount()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ToolService(context, null!, null!);

        // Act
        var result = service.GetToolListAsync(null!, 100).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Tool>>>(result);
        Assert.False(result.Success);
    }

}
