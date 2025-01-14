    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace RecipeManager.Models
    {
        public class Member
        {
            public int ID { get; set; }
            public string? IdentityUserId { get; set; }
            [ForeignKey("IdentityUserId")]
            public IdentityUser?     IdentityUser { get; set; }


            [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$", ErrorMessage =
     "Prenumele trebuie sa inceapa cu majuscula (ex. Ana sau Ana Maria sau AnaMaria")]

            [StringLength(30, MinimumLength = 3)]
            public string? FirstName { get; set; }
            public string? UserID { get; set; }

            [RegularExpression(@"^[A-Z]+[a-z\s]*$")]
            [StringLength(30, MinimumLength = 3)]
            public string? LastName { get; set; }

            [StringLength(70)]
            public string? Adress { get; set; }
            public string? Email { get; set; }

            [RegularExpression(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Telefonul trebuie sa fie de forma '0722-123-123' sau'0722.123.123' sau '0722 123 123'")]
            public string? Phone { get; set; }
            [Display(Name = "Full Name")]
            public string? FullName
            {
                get
                {
                    return FirstName + " " + LastName;
                }
            }
            public ICollection<Recipe>? RecipeCreateds { get; set; }

            public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();
        }
    }
