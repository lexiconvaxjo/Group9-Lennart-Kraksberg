using Project01.Models;
using Project01.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project01.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ListCarts()
        {
            var context = new AppDbContext();

            var Carts = context.Carts.ToList().Select(x => new CartVM
            {
                ID = x.ID,
                CartId = x.CartId,
                ItemId = x.ItemId,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,
            }).ToList();

            return View(Carts);
        }



        public ActionResult RenderCart(CartVM p)
        {
            return PartialView("_cartPart", p);
        }

        [HttpPost]
        public ActionResult SearchCart(string Search)
        {
            var context = new AppDbContext();

            var Carts = context.Carts.ToList().Select(x => new CartVM
            {
                ID = x.ID,
                CartId = x.CartId,
                ItemId = x.ItemId,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,

            }).ToList();

            List<CartVM> searchList = new List<CartVM>();
            int numSearch = 0;

            if (Carts == null)
            {
                return View("ListCarts", searchList);
            }
            else
            {
                foreach (var item in Carts)
                {
                    if (Search == "")
                    {
                        searchList.Add(item);
                    }
                    else
                    {
                        numSearch = int.Parse(Search);
                        if (item.ID == numSearch)
                            searchList.Add(item);
                    }

                }
                return View("ListCarts", searchList);
            }

        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddCart(CartVM model)
        {
            if (ModelState.IsValid)
            {
                var context = new AppDbContext();

                var cart = new Cart();

                cart.CartId = model.CartId;
                cart.ItemId = model.ItemId;
                cart.Price = model.Price;
                cart.Quantity = model.Quantity;
                cart.DateCreated = DateTime.Now;

                context.Carts.Add(cart);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Cart added.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Something went wrong!";
                    return View();
                }
            }
            // Not OK
            return View();
        }




        public ActionResult Delete(int search)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == search);

            context.Carts.Remove(ca);
            var affectedRows = context.SaveChanges();

            if (affectedRows > 0)
            {
                ViewBag.Message = "Cart " + search + " deleted.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
                return View();
            }

        }


    }
}