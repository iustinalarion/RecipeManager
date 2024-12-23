namespace RecipeManager.Models
{
    public class FavoriteRecipe
    {
        public int ID { get; set; } // Primary Key
        public int MemberID { get; set; } // Foreign Key
        public Member? Member { get; set; } // Navigation property
        public int RecipeID { get; set; } // Foreign Key
        public Recipe? Recipe { get; set; } // Navigation property
    }
}