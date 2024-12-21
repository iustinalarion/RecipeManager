using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.RecipeCreateds
{
    public class EditModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public EditModel(RecipeManager.Data.RecipeManagerContext context)
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

            var recipecreated =  await _context.RecipeCreated.FirstOrDefaultAsync(m => m.ID == id);
            if (recipecreated == null)
            {
                return NotFound();
            }
            RecipeCreated = recipecreated;
           ViewData["MemberID"] = new SelectList(_context.Member, "ID", "ID");
           ViewData["RecipeID"] = new SelectList(_context.Recipe, "ID", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RecipeCreated).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeCreatedExists(RecipeCreated.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RecipeCreatedExists(int id)
        {
            return _context.RecipeCreated.Any(e => e.ID == id);
        }
    }
}
