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

        public async Task<IActionResult> OnPostFavoriteAsync(int id)
        {
            // Get the logged-in user's MemberID
            var memberIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(memberIdString) || !int.TryParse(memberIdString, out int memberId))
            {
                // Redirect if the user is not logged in
                return RedirectToPage("./Index");
            }

            // Check if the favorite already exists
            var existingFavorite = await _context.FavoriteRecipe
                .FirstOrDefaultAsync(f => f.RecipeID == id && f.MemberID == memberId);

            if (existingFavorite == null)
            {
                // Add the favorite to the database
                var favorite = new FavoriteRecipe
                {
                    MemberID = memberId,
                    RecipeID = id
                };
                _context.FavoriteRecipe.Add(favorite);
                await _context.SaveChangesAsync();
            }

            // Redirect back to the Details page
            return RedirectToPage("./Details", new { id });
        }
    }
}
