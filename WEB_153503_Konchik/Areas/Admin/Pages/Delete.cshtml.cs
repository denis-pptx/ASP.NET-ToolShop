using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class DeleteModel : PageModel
    {
        private readonly IToolService _toolService;

        public DeleteModel(IToolService toolService)
        {
            _toolService = toolService;
        }

        [BindProperty]
        public Tool Tool { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _toolService.GetToolByIdAsync(id.Value);
            if (!response.Success)
            {
                return NotFound();
            }
            else
            {
                Tool = response.Data!;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _toolService.DeleteToolAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
