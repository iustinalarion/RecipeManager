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

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Member.FirstOrDefaultAsync(m => m.IdentityUserId == userId);

            if (member == null)
            {
                return RedirectToPage("/Error");
            }

            // Apply Include before Select to avoid the error
            FavoriteRecipes = await _context.FavoriteRecipe
                .Where(fr => fr.MemberID == member.ID)
                .Include(fr => fr.Recipe)
                .ThenInclude(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
                .Select(fr => fr.Recipe)
                .ToListAsync();

            return Page();
        }




    }
}
    