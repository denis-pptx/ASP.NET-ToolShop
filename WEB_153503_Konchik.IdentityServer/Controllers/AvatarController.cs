using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WEB_153503_Konchik.IdentityServer.Models;

namespace WEB_153503_Konchik.IdentityServer.Controllers;

[Route("[controller]")]
[Authorize] 
[ApiController]
public class AvatarController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<ApplicationUser> _userManager;

    public AvatarController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
    {
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvatar()
    {
        var userId = _userManager.GetUserId(User);
        var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        // Поиск всех файлов с расширениями изображений для данного пользователя
        var imageFiles = Directory.GetFiles(imagesPath, $"{userId}.*");

        if (imageFiles.Length > 0)
        {
            // Выбираем первый найденный файл 
            var imagePath = imageFiles[0];

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(imagePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(fileBytes, contentType);
        }
        else
        {
            // Если изображений нет, возвращаем файл-заменитель
            var placeholderPath = Path.Combine(imagesPath, "default-profile-picture.png");
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(placeholderPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var placeholderBytes = await System.IO.File.ReadAllBytesAsync(placeholderPath);
            return File(placeholderBytes, contentType);
        }
    }
}