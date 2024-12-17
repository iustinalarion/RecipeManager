namespace RecipeManager.Models
{
    public class RecipeCategory
    {

        public int Id { get; set; }

        public int RecipeID { get; set; }

        public Recipe Recipe { get; set; }

        public int CategoryID { get; set; }

        public Category Category { get; set; }
    }
}
