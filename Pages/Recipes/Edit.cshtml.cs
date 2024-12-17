﻿using System;
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
    public class EditModel : RecipeCategoriesPageModel
    {
        private readonly RecipeManagerContext _context;

        public EditModel(RecipeManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        [BindProperty]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load Recipe with related data
            Recipe = await _context.Recipe
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            // Load Categories and Ingredients
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
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (recipeToUpdate == null)
            {
                return NotFound();
            }

            // Update the Recipe fields
            if (await TryUpdateModelAsync<Recipe>(
                recipeToUpdate,
                "Recipe",
                r => r.Title, r => r.Description, r => r.PreparationTime, r => r.DateCreated))
            {
                // Update Categories
                UpdateBookCategories(_context, selectedCategories, recipeToUpdate);

                // Update Ingredients
                UpdateRecipeIngredients(recipeToUpdate);

                // Save changes
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Repopulate Categories and Ingredients if ModelState is invalid
            PopulateAssignedCategoryData(_context, recipeToUpdate);
            Ingredients = recipeToUpdate.Ingredients.ToList();
            return Page();
        }

        private void UpdateRecipeIngredients(Recipe recipeToUpdate)
        {
            var updatedIngredients = new List<Ingredient>();

            // Iterate over the Ingredients list sent from the form
            for (int i = 0; i < Ingredients.Count; i++)
            {
                var ingredient = Ingredients[i];
                if (!string.IsNullOrWhiteSpace(ingredient.Name) &&
                    !string.IsNullOrWhiteSpace(ingredient.Quantity) &&
                    !string.IsNullOrWhiteSpace(ingredient.Unit))
                {
                    updatedIngredients.Add(new Ingredient
                    {
                        ID = ingredient.ID, // Preserve ID for existing ingredients
                        Name = ingredient.Name,
                        Quantity = ingredient.Quantity,
                        Unit = ingredient.Unit,
                        RecipeID = recipeToUpdate.ID
                    });
                }
            }

            // Remove existing ingredients
            _context.Ingredient.RemoveRange(recipeToUpdate.Ingredients);

            // Add updated ingredients
            recipeToUpdate.Ingredients = updatedIngredients;
        }
    }
}