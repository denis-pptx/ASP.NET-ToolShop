using Moq;
using WEB_153503_Konchik.Domain.Entities;
using WEB_153503_Konchik.Domain.Models;
using WEB_153503_Konchik.Services.ToolService;
using WEB_153503_Konchik.Services.ToolCategoryService;
using WEB_153503_Konchik.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace WEB_153503_Konchik.Tests;

class CategoryComparer : IEqualityComparer<Category>
{
    public bool Equals(Category? x, Category? y)
    {
        if (x == null || y == null)
            return false;
        

        if (x == y)
            return true;
        
        return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
    }

    public int GetHashCode([DisallowNull] Category obj)
    {
        return HashCode.Combine(obj.Id, obj.Name, obj.NormalizedName);
    }
}
   

public class ProductControllerTests
{
    private List<Category> TestCategories => new List<Category>()
    {
        new() { Id = 1, Name = "Бетонные работы", NormalizedName = "concrete-works" },
        new() { Id = 2, Name = "Отделочные работы", NormalizedName = "finishing-works" }
    };

    private List<Tool> TestTools => new List<Tool>()
    {
        new () { Id = 1, Name = "Бетоносмеситель", Price = 450.0, CategoryId = 1 },
        new () { Id = 2, Name = "Стеклорез", Price = 25.0, CategoryId = 2 }
    };

    [Fact]
    public void IndexReturns404WhenCategoriesAreReceivedUnsuccessfully()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>> { Success = false });

        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>> { Success = true });

        var controller = new ProductController(categoryService.Object, toolService.Object);

        // Act.
        var result = controller.Index(null).Result;

        // Assert.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public void IndexReturns404WhenToolsAreReceivedUnsuccessfully()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>> { Success = true });

        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>> { Success = false });

        var controller = new ProductController(categoryService.Object, toolService.Object);

        // Act.
        var result = controller.Index(null).Result;

        // Assert.
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public void IndexViewDataContainsCategories()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>> 
            { 
                Success = true, 
                Data = TestCategories 
            });

        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>> 
            { 
                Success = true, 
                Data = new() { Items = TestTools } 
            });

     
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

        var controller = new ProductController(categoryService.Object, toolService.Object) 
        { 
            ControllerContext = new ControllerContext() 
            { 
                HttpContext = mockHttpContext.Object 
            } 
        };

        // Act.
        var result = controller.Index(null).Result;

        // Assert.
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(TestCategories, (viewResult.ViewData["categories"] as List<Category>)!, new CategoryComparer());
    }

    [Fact]
    public void IndexViewDataContainsValidCurrentCategoryWhenCategoryParameterIsNull()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>> { Success = true });

        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>> { Success = true });

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

        var controller = new ProductController(categoryService.Object, toolService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            }
        };

        // Act.
        var result = controller.Index(null).Result;

        // Assert.
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewData["currentCategory"] as Category);
    }

    
    [Fact]
    public void IndexViewDataContainsValidCurrentCategoryWhenCategoryParameterIsNotNull()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>>
            {
                Success = true,
                Data = TestCategories
            });

        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>>
            {
                Success = true,
                Data = new() { Items = TestTools }
            });


        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

        var controller = new ProductController(categoryService.Object, toolService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            }
        };

        // Act.
        var result = controller.Index("concrete-works").Result;

        // Assert.
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(TestCategories.First(c => c.NormalizedName == "concrete-works"), 
            (viewResult.ViewData["currentCategory"] as Category)!, new CategoryComparer());
    }

    [Fact]
    public void IndexRightModel()
    {
        // Arrange.
        var categoryService = new Mock<IToolCategoryService>();
        categoryService.Setup(m => m.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>>
            {
                Success = true,
                Data = TestCategories
            });

        var model = new ListModel<Tool>() { Items = TestTools };
        var toolService = new Mock<IToolService>();
        toolService.Setup(m => m.GetToolListAsync(It.IsAny<string?>(), It.IsAny<int>()))
            .ReturnsAsync(new ResponseData<ListModel<Tool>>
            {
                Success = true,
                Data = model
            });


        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

        var controller = new ProductController(categoryService.Object, toolService.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            }
        };

        // Act.
        var result = controller.Index(null).Result;

        // Assert.
        var viewResult = Assert.IsType<ViewResult>(result);
        var modelResult = Assert.IsType<ListModel<Tool>>(viewResult.Model);
        Assert.Equal(model, modelResult);
    }
}