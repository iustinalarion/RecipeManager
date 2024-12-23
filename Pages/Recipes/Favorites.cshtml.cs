using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.Recipes
{
    public class FavoritesModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public FavoritesModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<Recipe> FavoriteRecipes { get; set; } = new List<Recipe>();
        public RecipeData RecipeD { get; set; }
        public int RecipeID { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(int? id)
        {
            // Get the logged-in user's MemberID
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberId))
            {
                RecipeD = new RecipeData { Recipes = new List<Recipe>() };
                return;
            }

            // Base query to fetch favorite recipes for the logged-in member
            IQueryable<Recipe> favoriteRecipeQuery = _context.FavoriteRecipe
                .Where(f => f.Member.UserID == memberId) // Use the correct MemberID/UserID filter
                .Include(f => f.Recipe)
                .ThenInclude(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
                .Select(f => f.Recipe)
                .AsNoTracking();

            // Apply search filter if needed
            if (!string.IsNullOrEmpty(SearchString))
            {
                favoriteRecipeQuery = favoriteRecipeQuery.Where(r => r.Title.Contains(SearchString)
                                                                  || r.Description.Contains(SearchString));
            }

            // Populate RecipeData with the filtered recipes
            RecipeD = new RecipeData
            {
                Recipes = await favoriteRecipeQuery.OrderBy(r => r.Title).ToListAsync()
            };

            // If a specific recipe ID is selected, load its categories
            if (id != null)
            {
                RecipeID = id.Value;
                Recipe recipe = RecipeD.Recipes
                    .Where(r => r.ID == id.Value).SingleOrDefault();
                RecipeD.Categories = recipe?.RecipeCategories.Select(rc => rc.Category) ?? new List<Category>();
            }
        }
    }
}