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

namespace RecipeManager.Pages.Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public IndexModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<Ingredient> Ingredients { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? RecipeID { get; set; } // Selected Recipe ID for filtering

        public SelectList Recipes { get; set; } = default!; // Dropdown for recipes

        public async Task OnGetAsync()
        {
            // Fetch recipes for the dropdown
            Recipes = new SelectList(await _context.Recipe.ToListAsync(), "ID", "Title");

            // Base query for ingredients
            var ingredientsQuery = _context.Ingredient
                .Include(i => i.Recipe) // Include Recipe data
                .AsQueryable();

            // Apply filtering if a recipe is selected
            if (RecipeID.HasValue)
            {
                ingredientsQuery = ingredientsQuery.Where(i => i.RecipeID == RecipeID.Value);
            }

            // Execute the query
            Ingredients = await ingredientsQuery.ToListAsync();
        }
    }
}