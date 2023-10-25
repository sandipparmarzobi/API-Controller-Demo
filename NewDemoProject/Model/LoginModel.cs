using System.ComponentModel.DataAnnotations;

namespace NewDemoProject.Model
{
    public class LoginModel 
    {
       
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
           public string Password { get; set; }

        
    }
}