using System.ComponentModel.DataAnnotations;

namespace RecipeManager.Models
{
    public class RecipeCreated
    {
        public int ID { get; set; }
        public int? MemberID { get; set; }
        public Member? Member { get; set; }
        public int? RecipeID { get; set; }
        public Recipe? Recipe { get; set; }
       
    }
}
