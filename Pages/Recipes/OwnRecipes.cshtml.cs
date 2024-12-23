using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;
using System.Security.Claims; // For accessing User claims

namespace RecipeManager.Pages.Recipes
{
    public class OwnRecipe : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public OwnRecipe(RecipeManager.Data.RecipeManagerContext context)
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
            // Get the current user's MemberID
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<Recipe> recipeQuery = _context.Recipe
                .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
                .AsNoTracking()
                .Where(r => r.Member.UserID == memberId); // Filter by the current user's MemberID

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
