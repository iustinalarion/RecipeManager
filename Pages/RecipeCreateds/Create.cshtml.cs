using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.RecipeCreateds
{
    public class CreateModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public CreateModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var recipeList = _context.Recipe
                    .Select(x => new
                    {
                        x.ID,
                        RecipeFullName = x.Title
                    });


        ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName");
        ViewData["RecipeID"] = new SelectList(recipeList, "ID", "RecipeFullName");
            return Page();
        }

        [BindProperty]
        public RecipeCreated RecipeCreated { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RecipeCreated.Add(RecipeCreated);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
