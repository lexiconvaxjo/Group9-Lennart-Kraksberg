using Project01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project01.VM;
using static Project01.App_start.AppUserManager;
using Project01.App_start;
using Microsoft.AspNet.Identity.Owin;

namespace Project01.Controllers
{
    public class ShopController : Controller
    {


        public ShopController()
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


        public ShopController(AppRole role, AppUserManager userManager, AppSignIn signIn)
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


        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// Show a list of available products to buy
        /// </summary>
        /// <returns></returns>
        public ActionResult ListShopItems()
        {

// --------------------------------------------------          
            // Save Session Id
            // Used as key in Carts table

            if (Session["SessId"] == null)
            {
                string wid = Session.SessionID.ToString();
                Session.Add("SessId", wid);
            }
// --------------------------------------------------          

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





        /// <summary>
        /// Show list of products, partial
        /// </summary>
        /// <param name="p">Item</param>
        /// <returns></returns>
        public ActionResult RenderShopItem(Item p)
        {
            return PartialView("_shopItem", p);
        }



        /// <summary>
        /// Show a list of products in the shopping cart
        /// </summary>
        /// <param name="p">CartVM</param>
        /// <returns></returns>
        public ActionResult RenderShopCart(CartVM p)
        {
            return PartialView("_shopCart", p);
        }


        /// <summary>
        /// The customer have clicked on the "Buy" button on the product list
        /// </summary>
        /// <param name="id">ItemId</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult BuyItem(int id = 0)
        {
            var context = new AppDbContext();
            var item = context.Items.FirstOrDefault(x => x.ItemId == id);


            if (ModelState.IsValid && item != null)
            {

                var cart = new Cart();

                cart.CartId = Session["SessId"] as string;
                cart.ItemId = item.ItemId;
                cart.Price = item.Price;
                cart.Quantity = 1;
                cart.DateCreated = DateTime.Now;

                context.Carts.Add(cart);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                  //  ViewBag.Message = "Item " + item.Name + " added.";
                    ViewData["Message"] = "Product " + item.Name + " added to your cart." ;
                    return PartialView("_shopItem", item);
                     // return PartialView("_showCartMsg", item);

                }
                else
                {
                 //   ViewBag.Message = "Something went wrong!";
                    ViewData["Message"] = "Something went wrong!";
                    return PartialView("_shopItem", item);
                }
            }
            // Not OK
            return PartialView("_shopItem", item);

        }

        /// <summary>
        /// Show a list of the products in the shopping cart
        /// </summary>
        /// <returns></returns>
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

                foreach (var item in Carts)
                {
                    var product = context.Items.FirstOrDefault(x => x.ItemId == item.ItemId);

                    if (item.CartId == Session["SessId"] as string && product != null)
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



        /// <summary>
        /// Deletes an row in the Cart
        /// </summary>
        /// <param name="search">ID</param>
        /// <returns></returns>
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



        /// <summary>
        /// Edit a row in the shopping cart, change of quantity
        /// </summary>
        /// <param name="p">CartVM</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID, ItemName, ItemDescription, Price, Quantity")]CartVM p)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == p.ID);

            if (ModelState.IsValid)
            {
                ca.Quantity = p.Quantity;
                var affectedRows = context.SaveChanges();

                return PartialView("_shopCart", p);
            }
            else
            {
                var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
                var cart2 = new CartVM();

                if (product != null)
                {
                    cart2.ID = ca.ID;
                    cart2.CartId = ca.CartId;
                    cart2.ItemId = ca.ItemId;
                    cart2.ItemName = product.Name;
                    cart2.ItemDescription = product.Description;
                    cart2.Price = ca.Price;
                    cart2.Quantity = ca.Quantity;
                    cart2.DateCreated = ca.DateCreated;
                }

                return PartialView("_shopCart", cart2);
            }
        }

        /// <summary>
        /// Edit a row in the shopping cart 
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCart(int id = 0)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == id);

            var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
            var cart2 = new CartVM();

            if (product != null)
            {
                cart2.ID = ca.ID;
                cart2.CartId = ca.CartId;
                cart2.ItemId = ca.ItemId;
                cart2.ItemName = product.Name;
                cart2.ItemDescription = product.Description;
                cart2.Price = ca.Price;
                cart2.Quantity = ca.Quantity;
                cart2.DateCreated = ca.DateCreated;
            }

            return PartialView("_EditShopCart", cart2);
        }





        /// <summary>
        /// Cancel the edit task of an row in the shopping cart
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == id);

            var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
            var cart2 = new CartVM();

            if (product != null)
            {
                cart2.ID = ca.ID;
                cart2.CartId = ca.CartId;
                cart2.ItemId = ca.ItemId;
                cart2.ItemName = product.Name;
                cart2.ItemDescription = product.Description;
                cart2.Price = ca.Price;
                cart2.Quantity = ca.Quantity;
                cart2.DateCreated = ca.DateCreated;
            }


            return PartialView("_shopCart", cart2);
        }

        /// <summary>
        /// Enter invoice address and place order
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckOut()
        {

            var invAdr  = new InvoiceAddressVM();

            // If logged in, get address from Identity user

            if (User.Identity.IsAuthenticated)
            {
                var context = new AppDbContext();
                var usid = User.Identity.GetUserName();
                var us = context.Users.FirstOrDefault(x => x.UserName == usid );

                if (us != null)
                {
                    invAdr.FirstName = us.FirstName;
                    invAdr.LastName = us.LastName;
                    invAdr.Email = us.Email;
                    invAdr.PhoneNumber = us.PhoneNumber;
                    invAdr.Country = us.Country;
                    invAdr.City = us.City;
                    invAdr.Address = us.Address;
                    invAdr.PostalCode = us.PostalCode;
                }


            }

            return View(invAdr);
        }

        [HttpPost]
        public ActionResult SubmitOrder([Bind(Include = "FirstName, LastName, Email, PhoneNumber, Country, City, Address, PostalCode")]InvoiceAddressVM a)

        {
            // Create Order

            decimal orderTotal = 0;
            decimal rowAmount = 0;

            var context = new AppDbContext();

            var order = new Order();

            order.OrderDate = DateTime.Now;
            order.Total = 0;
            var cId = Session["SessId"] as string;
            order.CartId = cId;
            context.Orders.Add(order);
            var affectedRows = context.SaveChanges();


            // Create OrderDetails

            // get order id from Order table
            var or = context.Orders.FirstOrDefault(x => x.CartId == cId);
            var wOrderId = or.OrderId;

            if (or != null)
            {
                var carts = context.Carts.ToList().Select(x => new Cart
                {
                    CartId = x.CartId,
                    ItemId = x.ItemId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    DateCreated = x.DateCreated,
                }).ToList();

                if (carts != null)
                {
                    foreach (var item in carts)
                    {
                        if (item.CartId == cId)
                        {
                            var od = new OrderDetail();
                            od.OrderId = wOrderId;
                            od.ItemId = item.ItemId;
                            od.Quantity = item.Quantity;
                            od.UnitPrice = item.Price;

                            // add to ordertotal
                            rowAmount = item.Price * item.Quantity;
                            orderTotal = orderTotal + rowAmount;

                            context.OrderDetails.Add(od);
                            affectedRows = context.SaveChanges();
                        }
                    }

                    // update total amount in Order table
                    or = context.Orders.FirstOrDefault(x => x.CartId == cId);
                    if (or != null)
                    {
                        or.Total = orderTotal;
                        context.SaveChanges();

                    }
                }


                // create or update Invoice Address

                var iAdr = new InvoiceAddress();

                iAdr.FirstName = a.FirstName;
                iAdr.LastName = a.LastName;
                iAdr.Email = a.Email;
                iAdr.PhoneNumber = a.PhoneNumber;
                iAdr.Country = a.Country;
                iAdr.City = a.City;
                iAdr.Address = a.Address;
                iAdr.PostalCode = a.PostalCode;


                var ia = context.InvoiceAddresses.FirstOrDefault(x => x.Email == a.Email);

                if (ia == null)
                {
                    context.InvoiceAddresses.Add(iAdr);
                    affectedRows = context.SaveChanges();
                }
                else
                { 
                     context.SaveChanges();
                 }

         }








            //return View("CheckOut", a);
            return RedirectToAction("Index", "Home");

        }





    }
    }