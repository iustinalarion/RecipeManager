using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public IndexModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;
        public IList<Recipe> RecipesInCategory { get; set; } = new List<Recipe>();

        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; } // Selected Category ID

        public string SelectedCategoryName { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            // Fetch all categories
            Category = await _context.Category.ToListAsync();

            // Fetch recipes if a category is selected
            if (SelectedCategoryId.HasValue)
            {
                // Retrieve the name of the selected category
                SelectedCategoryName = await _context.Category
                    .Where(c => c.Id == SelectedCategoryId.Value)
                    .Select(c => c.CategoryName)
                    .FirstOrDefaultAsync() ?? "Selected";

                // Retrieve recipes for the selected category
                RecipesInCategory = await _context.Recipe
                    .Where(r => r.RecipeCategories
                        .Any(rc => rc.CategoryID == SelectedCategoryId.Value))
                    .ToListAsync();
            }
        }
    }
}