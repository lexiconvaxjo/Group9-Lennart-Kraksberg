using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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






    }
}