using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.Recipes
{
    [Authorize]
    public class EditModel : RecipeCategoriesPageModel
    {
        private readonly RecipeManagerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(RecipeManagerContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        [BindProperty]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        [BindProperty]
        public IFormFile Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            PopulateAssignedCategoryData(_context, Recipe);
            Ingredients = Recipe.Ingredients?.ToList() ?? new List<Ingredient>();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeToUpdate = await _context.Recipe
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (recipeToUpdate == null)
            {
                return NotFound();
            }

            // Handle image upload
            if (Image != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(recipeToUpdate.ImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, recipeToUpdate.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new image
                Directory.CreateDirectory(uploadsFolder);
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                // Update the ImagePath
                recipeToUpdate.ImagePath = $"Images/{uniqueFileName}";
            }

            // Update recipe fields
            await TryUpdateModelAsync(recipeToUpdate, "Recipe", r => r.Title, r => r.Description, r => r.PreparationTime, r => r.DateCreated);

            // Update categories
            UpdateBookCategories(_context, selectedCategories, recipeToUpdate);

            // Update ingredients
            UpdateRecipeIngredients(recipeToUpdate);

            // Save changes
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void UpdateRecipeIngredients(Recipe recipeToUpdate)
        {
            var updatedIngredients = new List<Ingredient>();

            for (int i = 0; i < Ingredients.Count; i++)
            {
                var ingredient = Ingredients[i];
                if (!string.IsNullOrWhiteSpace(ingredient.Name) &&
                    !string.IsNullOrWhiteSpace(ingredient.Quantity) &&
                    !string.IsNullOrWhiteSpace(ingredient.Unit))
                {
                    updatedIngredients.Add(new Ingredient
                    {
                        ID = ingredient.ID,
                        Name = ingredient.Name,
                        Quantity = ingredient.Quantity,
                        Unit = ingredient.Unit,
                        RecipeID = recipeToUpdate.ID
                    });
                }
            }

            // Remove old ingredients
            _context.Ingredient.RemoveRange(recipeToUpdate.Ingredients);

            // Add new ingredients
            recipeToUpdate.Ingredients = updatedIngredients;
        }
    }
}
