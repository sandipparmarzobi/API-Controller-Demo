using System.ComponentModel.DataAnnotations;

namespace NewDemoProject.Model
{
    public class LoginModel 
    {
       
            [Required]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Passwrod is required")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!*])[A-Za-z\d@#$%^&+=!*]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe{ get; set; }
    }
}