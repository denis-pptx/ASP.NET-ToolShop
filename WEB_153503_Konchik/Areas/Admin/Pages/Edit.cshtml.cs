using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_153503_Konchik.API.Data;
using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.Areas.Admin.Pages
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly IToolService _toolService;

        public EditModel(IToolService toolService)
        {
            _toolService = toolService;
        }

        [BindProperty]
        public Tool Tool { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _toolService.GetToolByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }
            Tool = response.Data!;

            return Page(); 
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _toolService.UpdateToolAsync(Tool.Id, Tool, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> ToolExists(int id)
        {
            var response = await _toolService.GetToolByIdAsync(id);
            return response.Success;
        }
    }
}
