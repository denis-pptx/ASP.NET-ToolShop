using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToolsController : ControllerBase
{
    private readonly IToolService _toolService;
    public ToolsController(IToolService toolService, IHttpContextAccessor httpContextAccessor)
    {
        _toolService = toolService;

        var _httpContext = httpContextAccessor.HttpContext!;
        var token = _httpContext.GetTokenAsync("access_token").Result;
    }

    // GET: api/Tools
    [HttpGet("{pageNo:int}")]
    [HttpGet("{category?}/{pageNo:int?}/")]
    public async Task<ActionResult<ResponseData<List<Tool>>>> GetTools(string? category, int pageNo = 1, int pageSize = 3)
    {
        var result = await _toolService.GetToolListAsync(category, pageNo, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET: api/Tools/tool5
    [HttpGet("tool{id}")]
    public async Task<ActionResult<ResponseData<Tool>>> GetTool(int id)
    {
        var result = await _toolService.GetToolByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // PUT: api/Tools/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<Tool>>> PutTool(int id, Tool tool)
    {
        var user = User;
        try
        {
            await _toolService.UpdateToolAsync(id, tool);
        }
        catch (Exception ex)
        {
            return NotFound(new ResponseData<Tool>()
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            });
        }

        return Ok(new ResponseData<Tool>()
        {
            Data = tool,
        });
    }

    // POST: api/Tools
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<ResponseData<Tool>>> PostTool(Tool tool)
    {
        var result = await _toolService.CreateToolAsync(tool);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // DELETE: api/Tools/5
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTool(int id)
    {
        try
        {
            await _toolService.DeleteToolAsync(id);
        }
        catch (Exception ex)
        {
            return NotFound(new ResponseData<Tool>()
            {
                Data = null,
                Success = false,
                ErrorMessage = ex.Message
            });
        }

        return NoContent();
    }

    // POST: api/Tools/5
    [Authorize(Roles = "admin")]
    [HttpPost("{id}")]
    public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
    {
        var response = await _toolService.SaveImageAsync(id, formFile);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    private async Task<bool> ToolExists(int id)
    {
        return (await _toolService.GetToolByIdAsync(id)).Success;
    }
}
