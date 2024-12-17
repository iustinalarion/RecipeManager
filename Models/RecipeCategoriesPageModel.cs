using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManager.Data;

namespace RecipeManager.Models
{
    public class RecipeCategoriesPageModel:PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } =new List<AssignedCategoryData>();
        public void PopulateAssignedCategoryData(RecipeManagerContext context,
        Recipe recipe)
        {
            var allCategories = context.Category;
            var recipeCategories = new HashSet<int>(
            recipe.RecipeCategories.Select(c => c.CategoryID)); //
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.Id,
                    Name = cat.CategoryName,
                    Assigned = recipeCategories.Contains(cat.Id)
                });
            }
        }
        public void UpdateBookCategories(RecipeManagerContext context,
        string[] selectedCategories, Recipe recipeToUpdate)
        {
            if (selectedCategories == null)
            {
                recipeToUpdate.RecipeCategories = new List<RecipeCategory>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>
            (recipeToUpdate.RecipeCategories.Select(c => c.Category.Id));

            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.Id.ToString()))
                {
                    if (!bookCategories.Contains(cat.Id))
                    {
                        recipeToUpdate.RecipeCategories.Add(
                        new RecipeCategory
                        {
                            RecipeID = recipeToUpdate.ID,
                            CategoryID = cat.Id
                        });
                    }
                }
                else
                {
                    if (bookCategories.Contains(cat.Id))
                    {
                        RecipeCategory bookToRemove
                        = recipeToUpdate
                        .RecipeCategories
                       .SingleOrDefault(i => i.CategoryID == cat.Id);
                        context.Remove(bookToRemove);
                    }
                }
            }
        }
    }
}
