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
    public class IndexModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public IndexModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get; set; } = default!;
        public RecipeData RecipeD { get; set; }
        public int RecipeID { get; set; }
        public int CategoryID { get; set; }

        // Search input property
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID)
        {
            IQueryable<Recipe> recipeQuery = _context.Recipe
                .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
                .AsNoTracking();

            // Apply search filter if SearchString is not null or empty
            if (!string.IsNullOrEmpty(SearchString))
            {
                recipeQuery = recipeQuery.Where(r => r.Title.Contains(SearchString)
                                                  || r.Description.Contains(SearchString));
            }

            RecipeD = new RecipeData
            {
                Recipes = await recipeQuery.OrderBy(r => r.Title).ToListAsync()
            };

            if (id != null)
            {
                RecipeID = id.Value;
                Recipe recipe = RecipeD.Recipes
                    .Where(i => i.ID == id.Value).Single();
                RecipeD.Categories = recipe.RecipeCategories.Select(s => s.Category);
            }
        }
    }
}