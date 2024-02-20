using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthProject.Models;
using AuthProject.Data;
using BCrypt.Net;

// using LINQCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthProject.Helpers;
// using System.Web.Mvc;

namespace AuthProject.Controllers
{
    

    public class AccessController : Controller
    {  private readonly ApplicationDbContext _context;
        public AccessController(ApplicationDbContext context)
        {
            _context = context;
           
        }
        
        public IActionResult Login()
        {
            // VMLogin vMLogin = new VMLogin();
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
             if (!ModelState.IsValid) return View(modelLogin);

          Boolean login=false;
            // _context.Users.

          User res = _context.Users.FirstOrDefault(u => u.Email == modelLogin.Email);
         if (res == null || !BCrypt.Net.BCrypt.Verify(modelLogin.Password, res.Password)){
            login=false;
         }else
            login=true;

            if (login)
            { 
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.NameIdentifier, res.Email),
                    new Claim(ClaimTypes.Name,res.Name)
                
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme );

                AuthenticationProperties properties = new AuthenticationProperties() { 
                
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
            
                return RedirectToAction("Index", "Home");
            }



            ViewData["ValidateMessage"] = "email or password not found";
            return View();
        }

        
        [HttpPost]
        public JsonResult LoginWithAjax(VMLogin modelLogin)
        {
            Console.WriteLine(modelLogin.Email);
            Console.WriteLine(modelLogin.Password);
           Boolean login=false;
            // _context.Users.
          AutoMapperProfile autoMapperProfile = new AutoMapperProfile();
          User res = _context.Users.FirstOrDefault(u => u.Email == modelLogin.Email);
         if (res == null || !autoMapperProfile.Encrypt(modelLogin.Password).Equals(res.Password)){
            login=false;
         }else
            login=true;

            if (login)
            { 
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.NameIdentifier, res.Email),
                    new Claim(ClaimTypes.Name,res.Name)
                
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme );

                AuthenticationProperties properties = new AuthenticationProperties() { 
                
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
            
            return Json(new { isValid = true, message = "login successfuly" });
                // return RedirectToAction("Index", "Home");
            }

            //  ViewData["ValidateMessage"] = "email or password not found";
            return Json(new { isValid = false, message = "email or password not correct" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  JsonResult LoginAJX(string param , [Bind("Email,Password,KeepLoggedIn")] VMLogin modelLogin)
        {
            AutoMapperProfile autoMapperProfile = new AutoMapperProfile();
            Console.WriteLine("param"+param);
            // if (!ModelState.IsValid) return View(modelLogin);

          Boolean login=false;
            // _context.Users.

          User res = _context.Users.FirstOrDefault(u => u.Email == modelLogin.Email);
 bool result = autoMapperProfile.Decrypt(modelLogin.Password).Equals(res.Password);
         if (res == null || !result){
            login=false;
         }else
            login=true;

            if (login)
            { 
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.NameIdentifier, res.Email),
                    new Claim(ClaimTypes.Name,res.Name)
                
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme );

                AuthenticationProperties properties = new AuthenticationProperties() { 
                
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
            
            return Json(new { isValid = true, message = "login successfuly" });
                // return RedirectToAction("Index", "Home");
            }

            //  ViewData["ValidateMessage"] = "email or password not found";
            return Json(new { isValid = false, message = "email or password not foundddd" });
            // ViewData["ValidateMessage"] = "email or password not found";
            //  return View();
        }
    }
}
