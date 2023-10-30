using System.ComponentModel.DataAnnotations;
            
namespace NewDemoProject.Model
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passwrod is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!*])[A-Za-z\d@#$%^&+=!*]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.")]
        [DataType(DataType.Password)]   
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}