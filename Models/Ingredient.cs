using RecipeManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class Ingredient
    {
        public int ID { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string? Name { get; set; } // Marked as nullable with '?'

        [Required]
        [StringLength(20)]
        public string? Quantity { get; set; } // Marked as nullable with '?'

        [Required]
        [StringLength(10)]
        public string? Unit { get; set; } // Marked as nullable with '?'

        public int RecipeID { get; set; } // Foreign Key
        public Recipe? Recipe { get; set; } // Navigation property marked nullable
    }
}