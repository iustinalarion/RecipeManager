using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.RecipeCreateds
{
    public class DeleteModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public DeleteModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RecipeCreated RecipeCreated { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipecreated = await _context.RecipeCreated.FirstOrDefaultAsync(m => m.ID == id);

            if (recipecreated == null)
            {
                return NotFound();
            }
            else
            {
                RecipeCreated = recipecreated;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipecreated = await _context.RecipeCreated.FindAsync(id);
            if (recipecreated != null)
            {
                RecipeCreated = recipecreated;
                _context.RecipeCreated.Remove(RecipeCreated);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
