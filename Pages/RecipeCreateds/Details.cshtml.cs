﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly RecipeManager.Data.RecipeManagerContext _context;

        public DetailsModel(RecipeManager.Data.RecipeManagerContext context)
        {
            _context = context;
        }

        public RecipeCreated RecipeCreated { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipecreated = await _context.RecipeCreated.FirstOrDefaultAsync(m => m.ID == id);
            if (recipecreated == null)
            {
                return NotFound();
            }
            else
            {
                RecipeCreated = recipecreated;
            }
            return Page();
        }
    }
}
