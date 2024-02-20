using System.ComponentModel.DataAnnotations;
namespace AuthProject.Models
{
    public class VMLogin
    {
        
       [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
