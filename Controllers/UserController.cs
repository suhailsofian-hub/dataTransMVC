using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthProject.Data;
using AuthProject.Models;
using AuthProject.ViewModel;
using static AuthProject.Helpers.Helper;

using Microsoft.EntityFrameworkCore;
using AuthProject.Helpers;
using AuthProject.Interfaces;
// using BCrypt.Net;
namespace AuthProject.Controllers
{
    
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        // private readonly ApplicationDbContext _context;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
           
        }

        
        public async Task<IActionResult> Index()
        {
            var users =await _userRepository.GetAllAsync();
            return View(users);          
        }

     private async Task<User> getUser(int id)
    {
        var user =await _userRepository.GetByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }


        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditUser(int id = 0)
        { Console.WriteLine("AddOrEditUser");
           UserViewModel userViewModel =new UserViewModel();
            if (id == 0){
                return View(userViewModel);
            }
            else
            {
                
                AutoMapperProfile autoMapperProfile = new AutoMapperProfile();
                var users =await _userRepository.GetByIdAsync(id);

                userViewModel.Id =  users.Id;
                userViewModel.Email = users.Email;
                userViewModel.Password = autoMapperProfile.Encrypt(users.Password);
                userViewModel.ConfirmPassword = autoMapperProfile.Encrypt(users.ConfirmPassword);
                userViewModel.Name = users.Name;
            
                if (userViewModel == null)
                {
                    return NotFound();
                }
               
                return View(userViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditUser(int id, [Bind("Id,Email,Name,Password,ConfirmPassword")] UserViewModel user)
        {
            User myuser =new User();
            
                AutoMapperProfile autoMapperProfile = new AutoMapperProfile();
                // Console.WriteLine("RoleIds22");
                //Insert
                if (id == 0)
                {
                    
                 if (ModelState.IsValid)
                  {
                     myuser = new User()
            {
                Email = user.Email,
                Password = autoMapperProfile.Encrypt(user.Password), 

                Name = user.Name,
                ConfirmPassword = autoMapperProfile.Encrypt(user.ConfirmPassword),
               
            };
           
                    Console.WriteLine("save myuser");
                    _userRepository.Add(myuser);
                    // await _context.SaveChangesAsync();
                 Console.WriteLine("result000--> ");
                // Console.WriteLine("result--> "+Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _userRepository.GetAll()) });
                }
                
                Console.WriteLine("fail ");
                // user.Id = 0;
                 return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditUser", user) });
            }
                //Update
                else
                {
                    // Console.WriteLine("id-->  "+id);
                    // Console.WriteLine("user-->  "+user);
                    try
                    {
                         myuser =await getUser(id);
                  // hash password if it was entered
                 if (!string.IsNullOrEmpty(user.Password)){
                    Console.WriteLine("IsNullOrEmpty Password"+user.Password);
                      myuser.Password = autoMapperProfile.Encrypt(user.Password);
                   }

            if (!string.IsNullOrEmpty(user.ConfirmPassword)){
            myuser.ConfirmPassword = autoMapperProfile.Encrypt(user.ConfirmPassword);
            }

           if (!string.IsNullOrEmpty(user.Name))
            myuser.Name = user.Name;
            if (!string.IsNullOrEmpty(user.Email))
            myuser.Email = user.Email;

            _userRepository.Update(myuser);
                        // await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionModelExists(myuser.Id))
                        { return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditUser", myuser) }); }
                        else
                        { throw; }
                    }

                    Console.WriteLine("result000--> ");
                // Console.WriteLine("result--> "+Helper.RenderRazorViewToString(this, "_ViewAll", _context.Transactions.ToList()));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _userRepository.GetAll()) });
                }
            
           
        }


        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _userRepository.GetByIdAsync(id);
            _userRepository.Delete(userModel);
            // await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _userRepository.GetAll()) });
        }
 private bool TransactionModelExists(int id)
        {
            return _userRepository.exist(id);
        }
 }


}
