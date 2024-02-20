using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthProject.Data;
using AuthProject.Models;
// using MVC6Crud.Repository;
using AuthProject.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using static AuthProject.Helpers.Helper;

using Microsoft.EntityFrameworkCore;
using AuthProject.Helpers;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
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
            // UserListViewModel userListViewModel =new UserListViewModel();
            // List<UserListViewModel> userListViewModelList =new ArrayList<UserListViewModel>();
            // List<string> rols= new List<string>();
            var users =await _userRepository.GetAllAsync();
            // List<Role> roles =await _context.Roles.ToListAsync();
//             foreach (var item in roles)
//             {

// rols = item.RoleName

                
//             }
            //   foreach (var item in users)
            //   { 
                
            //     userListViewModel.Email=item.Email;
            //     userListViewModel.Password=item.Password;
            //     userListViewModel.ConfirmPassword=item.ConfirmPassword;
            //     userListViewModel.Roles=roles;
            //     userListViewModelList.Add(userListViewModel);
            //   }
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
        { Console.WriteLine("ddddddddddddddddddddddd");
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
            // List<int> rrs =new List<int>();
            //  List<int> rrs2 =new List<int>();
            // Role r=new Role();
            // user.RoleItms.Add(r);
            // var model = user.RoleIds.Where(p => p.Contains(p.));
            // Attach Validation Error Message to the Model on validation failure.
    
             
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
                    Console.WriteLine("id-->  "+id);
                    Console.WriteLine("user-->  "+user);
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
            
            // user.RolesItms =  GetAllRoles().Result.Select(k => new SelectListItem
            // {
            //     Text = k.RoleName,
            //     Value = Convert.ToString(k.Id)
            // }).ToList(); 
            // Console.WriteLine("result--> "+myuser.Email);
            // return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditUser", myuser) });
        }

         public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _userRepository.GetByIdAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
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

        public IActionResult Create()
        {
            UserViewModel userCreateViewModel = new UserViewModel();
            

            return View(userCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UserViewModel userCreateViewModel)
        {

            if (ModelState.IsValid){
                // Console.WriteLine("insiiiid1"+userCreateViewModel.Password);
                // Console.WriteLine("insiiiid2"+userCreateViewModel.ConfirmPassword);
                // string passwordHash = BCrypt.Net.BCrypt.HashPassword(userCreateViewModel.Password);
               var user = new User()
            {
                Email = userCreateViewModel.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userCreateViewModel.Password), 

                Name = userCreateViewModel.Name,
                ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(userCreateViewModel.ConfirmPassword)
            };
           
                _userRepository.Add(user);
                // _context.SaveChanges();

                //-----------------------------------------------------------------------------
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name,user.Name)
                
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme );

                AuthenticationProperties properties = new AuthenticationProperties() { 
                
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                //-----------------------------------------------------------------------------
                return RedirectToAction("Index", "Home");
            }else{
                TempData["SuccessMsg"] = "Fail to save data";
             return View(userCreateViewModel);
            }
        
        }

        // public async IActionResult Edit(int id)
        // {
        //     User productToEdit =await _userRepository.GetByIdAsync(id);
        //     if (productToEdit != null)
        //     {
        //         var userViewModel = new UserViewModel()
        //         {
        //             Id = productToEdit.Id,
        //             Email = productToEdit.Email,
        //             Name = productToEdit.Name,
        //             Password = productToEdit.Password,
        //             ConfirmPassword = productToEdit.ConfirmPassword
                    
                
        //         };
        //         return View(userViewModel);
        //     }
        //     else
        //     {
        //         return NotFound();
        //     }
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Edit(UserViewModel userViewModel)
        // {
        //     if (!ModelState.IsValid){
        //         // TempData["Error"] = "This email address is already in use";
        //         return View(userViewModel);
        //     }
             
            
        //     var user = new User()
        //     {
        //         Id = userViewModel.Id,
        //         Email = userViewModel.Email,
        //         Name = userViewModel.Name,
        //         Password = userViewModel.Password,
        //         ConfirmPassword = userViewModel.ConfirmPassword
                
        //     };
            
        //         _context.Users.Update(user);
        //         _context.SaveChanges();
        //         TempData["SuccessMsg"] = "User (" + user.Name + ") updated successfully !";
        //         return RedirectToAction("Index");
            

        //     // return View(userViewModel);
        // }

        // public IActionResult Delete(int? id)
        // {
        //     var productToEdit = _context.Users.Find(id);
        //     if (productToEdit != null)
        //     {
        //         var productViewModel = new ProductViewModel()
        //         {
        //             Id = productToEdit.Id,
        //             Name = productToEdit.Name,
        //             Description = productToEdit.Description,
        //             Price = productToEdit.Price,
        //             CategoryId = productToEdit.CategoryId,
        //             Color = productToEdit.Color,
        //             Image = productToEdit.Image,
        //             Category = (IEnumerable<SelectListItem>)_context.Categories.Select(c => new SelectListItem()
        //             {
        //                 Text = c.CategoryName,
        //                 Value = c.CategoryId.ToString()
        //             })
        //         };
        //         return View(productViewModel);
        //     }
        //     else {
        //         return RedirectToAction("Index");
        //     }            
        // }

    //     [HttpPost]
    //     [ValidateAntiForgeryToken]
    //     public IActionResult DeleteProduct(int? id)
    //     {
    //         var product = _context.Products.Find(id);
    //         if (product == null)
    //         {
    //             return NotFound();
    //         }
    //         _context.Products.Remove(product);
    //         _context.SaveChanges();
    //         TempData["SuccessMsg"] = "Product (" + product.Name + ") deleted successfully.";
    //         return RedirectToAction("Index");
    //     }
    }


}
