using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.Recipes
{
    public class DetailsModel : RecipeCategoriesPageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public DetailsModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include Ingredients and Categories
            Recipe = await _context.Recipe
                .Include(r => r.RecipeCategories).ThenInclude(rc => rc.Category)
                .Include(r => r.Ingredients) // Include Ingredients
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            PopulateAssignedCategoryData(_context, Recipe);
            return Page();
        }
    }
}
