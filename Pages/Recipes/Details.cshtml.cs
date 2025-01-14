        using System;
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

            Recipe = await _context.Recipe
                .Include(r => r.RecipeCategories)
                .ThenInclude(rc => rc.Category)
                .Include(r => r.Ingredients) // ✅ Include Ingredients
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }



        public async Task<IActionResult> OnPostFavoriteAsync(int recipeId)
        {
            if (recipeId == 0)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Member.FirstOrDefaultAsync(m => m.IdentityUserId == userId);

            if (member == null)
            {
                return NotFound();
            }

            var existingFavorite = await _context.FavoriteRecipe
                .FirstOrDefaultAsync(fr => fr.MemberID == member.ID && fr.RecipeID == recipeId);

            if (existingFavorite == null)
            {
                var favoriteRecipe = new FavoriteRecipe
                {
                    MemberID = member.ID,
                    RecipeID = recipeId
                };

                _context.FavoriteRecipe.Add(favoriteRecipe);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Favorites");
        }
    


    }
}
