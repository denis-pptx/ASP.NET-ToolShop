using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153503_Konchik.API.Data;
using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.Areas.Admin.Pages
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IToolService _toolService;
        public IndexModel(IToolService toolService)
        {
            _toolService = toolService;
        }

        public IList<Tool> Tools { get; set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            // var user = HttpContext.User;

            var responce = await _toolService.GetToolListAsync(null, pageNo);

            if (!responce.Success)
                return NotFound(responce.ErrorMessage ?? "");


            Tools = responce.Data?.Items!;
            CurrentPage = responce.Data?.CurrentPage ?? 0;
            TotalPages = responce.Data?.TotalPages ?? 0;

            return Page();

        }
    }
}
