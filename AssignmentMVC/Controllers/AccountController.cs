using Project01.App_start;
using Project01.Models;
using Project01.VM;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Project01.App_start.AppUserManager;

namespace Project01.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {


        public ActionResult LogOut()
        {
            SignIn.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }





        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }





        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddUser(AppUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    PostalCode = model.PostalCode,
                };

                var res = await UserManager.CreateAsync(user, model.Password);

                if (res.Succeeded)
                {
                    var regUser = await UserManager.FindByNameAsync(model.UserName);


                    if (model.Admin == true)
                        await UserManager.AddToRoleAsync(regUser.Id, "Admin");
                    else
                        await UserManager.AddToRoleAsync(regUser.Id, "User");


                    LogInVM vmod = new LogInVM();


                    vmod.UserName = model.UserName;
                    vmod.Password = model.Password;
                    await Login(vmod);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("RegUser", model);
                }
            }
            // Not OK
            return View("RegUser", model);
        }


        [AllowAnonymous]
        public ActionResult RegUser()
        {
            return View();
        }




        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }






        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }



        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LogInVM user)
        {
            if (ModelState.IsValid)
            {
                var status = await SignIn.PasswordSignInAsync(user.UserName, user.Password,
                    isPersistent: false, shouldLockout: true);



                switch (status)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Index", "Home");
                    case SignInStatus.Failure:
                        ViewBag.Message = "Wrong username or password";
                        break;
                    default:
                    break;

                }
            }
            // Not OK
            return View(user);
        }























        public AccountController()
        {
        }

        private AppRole _role;
        public AppRole RoleManager
        {
            get { return _role ?? HttpContext.GetOwinContext().Get<AppRole>(); }
            set { _role = value; }
        }



        private AppSignIn _signIn;
        public AppSignIn SignIn
        {
            get { return _signIn ?? HttpContext.GetOwinContext().Get<AppSignIn>(); }
            set { _signIn = value; }
        }



        private AppUserManager _userManager;
        public AppUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
            set { _userManager = value; }
        }



        public AccountController(AppRole role, AppUserManager userManager, AppSignIn signIn)
        {
            _role = role;
            _signIn = signIn;
            _userManager = userManager;
        }


    }
}