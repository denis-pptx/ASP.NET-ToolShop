using Moq;
using WEB_153503_Konchik.Domain.Entities;
using WEB_153503_Konchik.Domain.Models;
using WEB_153503_Konchik.Services.ToolService;
using WEB_153503_Konchik.Services.ToolCategoryService;
using WEB_153503_Konchik.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153503_Konchik.Tests;

public class ProductControllerTests
{
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
}

