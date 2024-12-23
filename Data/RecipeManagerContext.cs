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
        public DbSet<RecipeManager.Models.Category> Category { get; set; } = default!;
        public DbSet<RecipeManager.Models.Member> Member { get; set; } = default!;
        public DbSet<RecipeManager.Models.RecipeCreated> RecipeCreated { get; set; } = default!;
        public DbSet<RecipeManager.Models.FavoriteRecipe> FavoriteRecipe { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Member-Recipe (One-to-Many)
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Member)
                .WithMany(m => m.RecipeCreateds)
                .HasForeignKey(r => r.MemberID)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a Member is removed

            // FavoriteRecipe Relationships
            modelBuilder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.Member)
                .WithMany(m => m.FavoriteRecipes)
                .HasForeignKey(fr => fr.MemberID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid accidental data loss

            modelBuilder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.Recipe)
                .WithMany(r => r.FavoriteRecipes)
                .HasForeignKey(fr => fr.RecipeID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Composite Index for FavoriteRecipes (to ensure no duplicates)
            modelBuilder.Entity<FavoriteRecipe>()
                .HasIndex(fr => new { fr.MemberID, fr.RecipeID })
                .IsUnique();

            // Unique Constraint for UserID in Member
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.UserID)
                .IsUnique();
        }
    }
}
