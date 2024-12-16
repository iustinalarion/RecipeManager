using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Models;

namespace RecipeManager.Data
{
    public class RecipeManagerContext : DbContext
    {
        public RecipeManagerContext (DbContextOptions<RecipeManagerContext> options)
            : base(options)
        {
        }

        public DbSet<RecipeManager.Models.Ingredient> Ingredient { get; set; } = default!;
        public DbSet<RecipeManager.Models.Recipe> Recipe { get; set; } = default!;
    }
}
