using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public IndexModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; } = default!;
        public RecipeData RecipeD { get; set; }
        public int RecipeID { get; set; }
        public int CategoryID { get; set; }


        public async Task OnGetAsync(int? id, int? categoryID)
        {
            RecipeD = new RecipeData();

           
            RecipeD.Recipes = await _context.Recipe
            .Include(b => b.RecipeCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();
            if (id != null)
            {
                RecipeID = id.Value;
                Recipe recipe = RecipeD.Recipes
                .Where(i => i.ID == id.Value).Single();
                RecipeD.Categories = recipe.RecipeCategories.Select(s => s.Category);
            }
        }


    }
}
