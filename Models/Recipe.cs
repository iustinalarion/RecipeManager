using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class Recipe
    {
        public int ID { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string? Title { get; set; } // Marked as nullable with '?'

        [Required]
        [StringLength(1000)]
        public string? Description { get; set; } // Marked as nullable with '?'

        [Display(Name = "Preparation Time (minutes)")]
        [Range(1, 360)]
        public int PreparationTime { get; set; }

        public ICollection<Ingredient>? Ingredients { get; set; } // Nullable navigation property

        [DataType(DataType.Date)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now; // Default to now

        public ICollection<RecipeCategory>? RecipeCategories {  get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }

        public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();
    }
}