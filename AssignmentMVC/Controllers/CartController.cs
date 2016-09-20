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

            var Carts = context.Carts.ToList().Select(x => new Cart
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




        public ActionResult RenderCart(Cart p)
        {
            return PartialView("_cart", p);
        }



        [HttpPost]
        public ActionResult SearchCart(string Search)
        {
            var context = new AppDbContext();

            var Carts = context.Carts.ToList().Select(x => new Cart
            {
                ID = x.ID,
                CartId = x.CartId,
                ItemId = x.ItemId,
                Price = x.Price,
                Quantity = x.Quantity,
                DateCreated = x.DateCreated,

            }).ToList();

            List<Cart> findList = new List<Cart>();
           // int numSearch = 0;

            if (Carts == null)
            {
                return View("ListCarts", findList);
            }
            else
            {
                foreach (var item in Carts)
                {
                    if (Search == "")
                    {
                        findList.Add(item);
                    }
                    else
                    {
                      //  numSearch = int.Parse(Search);
                        if (item.CartId == Search)
                            findList.Add(item);
                    }

                }
                return View("ListCarts", findList);
            }

        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddCart(Cart model)
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

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID, CartId, ItemId, Price, Quantity, DateCreated")]Cart p)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == p.ID);

            if (ModelState.IsValid)
            {
                ca.CartId = p.CartId;
                ca.ItemId = p.ItemId;
                ca.Price = p.Price;
                ca.Quantity = p.Quantity;
                ca.DateCreated = p.DateCreated;
                var affectedRows = context.SaveChanges();

                return PartialView("_cart", p);


            }
            else
            {
                return PartialView("_cart", ca);
            }
        }

        [HttpPost]
        public ActionResult EditCart(int id = 0)
        {
            var context = new AppDbContext();

            var cart = context.Carts.FirstOrDefault(x => x.ID == id);
            return PartialView("_EditCart", cart);
        }



        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == id);
            return PartialView("_cart", ca);
        }




    }
}