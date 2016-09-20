using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project01.VM;

namespace Project01.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ListShopItems()
        {
            var context = new AppDbContext();

            var Items = context.Items.ToList().Select(x => new Item
            {
                ItemId = x.ItemId,
                Name = x.Name,
                Price = x.Price,
                Picture = x.Picture,
                Description = x.Description,
                StockQty = x.StockQty,
            }).ToList();

            return View(Items);
        }






        public ActionResult RenderShopItem(Item p)
        {
            return PartialView("_shopItem", p);
        }


        public ActionResult RenderShopCart(CartVM p)
        {
            return PartialView("_shopCart", p);
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult BuyItem(int id = 0)
        {
            var context = new AppDbContext();
            var item = context.Items.FirstOrDefault(x => x.ItemId == id);


            if (ModelState.IsValid && item != null)
            {

                var cart = new Cart();

                if (Request.IsAuthenticated)
                    cart.CartId = User.Identity.GetUserName();
                else
                    cart.CartId = Session.SessionID.ToString();


                cart.ItemId = item.ItemId;
                cart.Price = item.Price;
                cart.Quantity = 1;
                cart.DateCreated = DateTime.Now;

                context.Carts.Add(cart);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Item " + item.Name + " added.";
                      return PartialView("_shopItem", item);
                     // return PartialView("_showCartMsg", item);

                }
                else
                {
                    ViewBag.Message = "Something went wrong!";
                    return PartialView("_shopItem", item);
                }
            }
            // Not OK
            return PartialView("_shopItem", item);

        }


        public ActionResult ListShopCart()
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

            List<CartVM> Carts2 = new List<CartVM>();

            //
            if (Carts == null)
            {
                return View("ListShopCart", Carts2);
            }
            else
            {
                string wId;
                if (Request.IsAuthenticated)
                    wId = User.Identity.GetUserName();
                else
                    wId = Session.SessionID.ToString();

                foreach (var item in Carts)
                {
                    var product = context.Items.FirstOrDefault(x => x.ItemId == item.ItemId);

                    if (item.CartId == wId && product != null)
                    {
                        var cart2 = new CartVM();

                        cart2.ID = item.ID;
                        cart2.CartId = item.CartId;
                        cart2.ItemId = item.ItemId;
                        cart2.ItemName = product.Name;
                        cart2.ItemDescription = product.Description;
                        cart2.Price = item.Price;
                        cart2.Quantity = item.Quantity;
                        cart2.DateCreated = item.DateCreated; 

                        Carts2.Add(cart2);
                    }
                }
                return View("ListShopCart", Carts2);
            }

            //
        }








    }
}