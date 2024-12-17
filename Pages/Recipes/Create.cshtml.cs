using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManager.Data;
using RecipeManager.Models;

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

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
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

            // Add RecipeCategories to the Recipe
            Recipe.RecipeCategories = newRecipe.RecipeCategories;

            // Save Recipe
            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            // Save Ingredients associated with the Recipe
            foreach (var ingredient in Ingredients)
            {
                ingredient.RecipeID = Recipe.ID; // Link ingredient to the new recipe
                _context.Ingredient.Add(ingredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}