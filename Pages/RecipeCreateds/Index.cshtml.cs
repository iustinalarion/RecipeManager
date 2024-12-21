using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Data;
using RecipeManager.Models;

namespace RecipeManager.Pages.RecipeCreateds
{
    public class IndexModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public IndexModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public IList<RecipeCreated> RecipeCreated { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RecipeCreated = await _context.RecipeCreated
                .Include(r => r.Member)
                .Include(r => r.Recipe).ToListAsync();
        }
    }
}
