using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManager.Data;
using RecipeManager.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore; // For accessing user claims

namespace RecipeManager.Pages.Recipes
{
    public class CreateModel : RecipeCategoriesPageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public CreateModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var recipe = new Recipe();
            recipe.RecipeCategories = new List<RecipeCategory>();
            PopulateAssignedCategoryData(_context, recipe);

            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories, IFormFile Image)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Member.FirstOrDefaultAsync(m => m.UserID == userId);
            Recipe.Member = member;

            var newRecipe = new Recipe();
            if (selectedCategories != null)
            {
                newRecipe.RecipeCategories = new List<RecipeCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new RecipeCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newRecipe.RecipeCategories.Add(catToAdd);
                }
            }

            if (Image != null)
            {
                var imagePath = Path.Combine("wwwroot/images", Image.FileName);
                Directory.CreateDirectory("wwwroot/images");
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                Recipe.ImagePath = $"/images/{Image.FileName}";
            }

            Recipe.RecipeCategories = newRecipe.RecipeCategories;
            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            foreach (var ingredient in Ingredients)
            {
                ingredient.RecipeID = Recipe.ID;
                _context.Ingredient.Add(ingredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
