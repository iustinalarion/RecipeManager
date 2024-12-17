namespace RecipeManager.Models
{
    public class RecipeData
    {
        public IEnumerable<Recipe> Recipes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<RecipeCategory> RecipeCategories { get; set; }
    }
}
