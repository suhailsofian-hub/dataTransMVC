using AuthProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AuthProject.ViewModel
{
    public class UserViewModel
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "The Password field must be at least 6 characters long")]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword not match")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "The ConfirmPassword field must be at least 6 characters long")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }


    }
}
