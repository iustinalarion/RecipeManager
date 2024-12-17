using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;


namespace RecipeManager.Pages.Recipes
{
    public class EditModel : RecipeCategoriesPageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public EditModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipe
                .Include(b => b.RecipeCategories).ThenInclude(b => b.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

           
            if (Recipe == null)
            {
                return NotFound();
            }

            PopulateAssignedCategoryData(_context, Recipe);

            return Page();

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[]
selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var recipeToUpdate = await _context.Recipe
            .Include(i => i.RecipeCategories)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);
            if (recipeToUpdate == null)
            {
                return NotFound();
            }
           
            if (await TryUpdateModelAsync<Recipe>(
            recipeToUpdate,
            "Recipe",
            i => i.Title, i => i.Description,
            i => i.PreparationTime, i => i.Ingredients, i => i.DateCreated ))
            {
                UpdateBookCategories(_context, selectedCategories, recipeToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            
            UpdateBookCategories(_context, selectedCategories, recipeToUpdate);
            PopulateAssignedCategoryData(_context, recipeToUpdate);
            return Page();
        }
    }


       
    
}


