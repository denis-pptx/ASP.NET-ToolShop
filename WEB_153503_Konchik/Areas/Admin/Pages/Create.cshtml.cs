using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153503_Konchik.API.Data;
using WEB_153503_Konchik.Domain.Entities;

namespace WEB_153503_Konchik.Areas.Admin.Pages
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IToolService _toolService;

        public CreateModel(IToolService toolService)
        {
            _toolService = toolService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Tool Tool { get; set; } = default!;

        [BindProperty]
        public IFormFile Image { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _toolService.CreateToolAsync(Tool, Image);
            if (!response.Success)
            {
                return Page();
            }
            
            return RedirectToPage("./Index");
        }
    }
}
