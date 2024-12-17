namespace RecipeManager.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public ICollection<RecipeCategory>? RecipeCategories { get; set; }

    }
}
