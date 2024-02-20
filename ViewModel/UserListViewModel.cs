using AuthProject.Models;

namespace AuthProject.ViewModel
{
    public class UserListViewModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
       public string? Name { get; set; }
       public string Password { get; set; }
       public string ConfirmPassword { get; set; }
    //    public List<Role> Roles { get; set; }
    }
}
