using Project01.App_start;
using Project01.Models;
using Project01.VM;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Project01.App_start.AppUserManager;

namespace Project01.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        public async Task<ActionResult> Delete(string find)
        {
            var user = await UserManager.FindByEmailAsync(find);
            var res = await UserManager.DeleteAsync(user);

            return RedirectToAction("Index", "Home");
        }


        public ActionResult ListUsers()
        {
            var Users = UserManager.Users.ToList().Select(x => new UserVM
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                City = x.City,
                Country = x.Country,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Id = x.Id
            }).ToList();

            return View(Users);
        }


        [HttpPost]
        public ActionResult SearchUser(string Find)
        {
            var Users = UserManager.Users.ToList().Select(x => new UserVM
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                City = x.City,
                Country = x.Country,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Id = x.Id
            }).ToList();

            List<UserVM> findList = new List<UserVM>();

            if (Users == null)
            {
                return View("ListUsers", findList);
            }
            else
            {
                foreach (var item in Users)
                {
                    if (item.FirstName != null && item.LastName != null && item.Email != null)
                    {
                        if (item.FirstName.Contains(Find) || item.LastName.Contains(Find) || item.Email.Contains(Find))
                        {
                            findList.Add(item);
                        }
                    }
                }
                return View("ListUsers", findList);
            }

        }



        public ActionResult RenderUser(UserVM p)
        {
            return PartialView("_userPart", p);
        }




        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddUser(UserVM model, string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = UserName,
                    Email = model.Email,
                    Country = model.Country,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    PostalCode = model.PostalCode,
                };

                var res = await UserManager.CreateAsync(user, Password);

                if (res.Succeeded)
                {
                    var regUser = await UserManager.FindByNameAsync(UserName);
                    await UserManager.AddToRoleAsync(regUser.Id, "User");
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




























        public UserController()
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


        public UserController(AppRole role, AppUserManager userManager, AppSignIn signIn)
        {
            _role = role;
            _signIn = signIn;
            _userManager = userManager;
        }


        private AppUserManager _userManager;
        public AppUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
            set { _userManager = value; }
        }


    }
}
