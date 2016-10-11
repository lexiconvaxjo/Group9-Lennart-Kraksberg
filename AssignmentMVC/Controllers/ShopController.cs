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
        /// Show a list of products in the shopping cart
        /// for the order confirmation view
        /// </summary>
        /// <param name="p">CartVM</param>
        /// <returns></returns>
        public ActionResult RenderOrderConfirm(CartVM p)
        {
            return PartialView("_shopConfirm", p);
        }



        /// <summary>
        /// Show shopping history
        /// </summary>
        /// <param name="p">HistoryVM</param>
        /// <returns></returns>
        public ActionResult RenderOrderHistory(HistoryVM p)
        {
            return PartialView("_shopHistory", p);
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
            int affectedRows = 0;
            var ca = context.Carts.FirstOrDefault(x => x.ID == search);

            var cart2 = new CartVM();

            if (ca != null)
            {
                cart2.ID = ca.ID;
                cart2.ItemName = "Deleted";
                cart2.ItemDescription = " ";
                cart2.Price = 0;
                cart2.Quantity = 0;

                context.Carts.Remove(ca);
                affectedRows = context.SaveChanges();
            }

            if (affectedRows > 0)
            {
                ViewBag.Message = "Cart " + search + " deleted.";
             //   return RedirectToAction("Index", "Home");
                return PartialView("_shopCart", cart2);

            }
            else
            {
                ViewBag.Message = "Something went wrong!";
              //  return View();
                return PartialView("_shopCart", cart2);
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

            if (ModelState.IsValid && ca != null)
            {
                ca.Quantity = p.Quantity;
                var affectedRows = context.SaveChanges();

                return PartialView("_shopCart", p);
            }
            else
            {
                var cart2 = new CartVM();

                if (ca != null)
                {
                    cart2.ID = ca.ID;
                    cart2.CartId = ca.CartId;
                    cart2.ItemId = ca.ItemId;
                    cart2.Price = ca.Price;
                    cart2.Quantity = ca.Quantity;
                    cart2.DateCreated = ca.DateCreated;
                    var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
                    if (product != null)
                    {
                        cart2.ItemName = product.Name;
                        cart2.ItemDescription = product.Description;
                    }

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
            var cart2 = new CartVM();

            var context = new AppDbContext();
            var ca = context.Carts.FirstOrDefault(x => x.ID == id);


            if (ca != null)
            {
                var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
                cart2.ID = ca.ID;
                cart2.CartId = ca.CartId;
                cart2.ItemId = ca.ItemId;
                if (product != null)
                {
                    cart2.ItemName = product.Name;
                    cart2.ItemDescription = product.Description;
                }
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
            var cart2 = new CartVM();

            var ca = context.Carts.FirstOrDefault(x => x.ID == id);

            if (ca != null)
            {
                cart2.ID = ca.ID;
                cart2.CartId = ca.CartId;
                cart2.ItemId = ca.ItemId;
                cart2.Price = ca.Price;
                cart2.Quantity = ca.Quantity;
                cart2.DateCreated = ca.DateCreated;

                var product = context.Items.FirstOrDefault(x => x.ItemId == ca.ItemId);
                if (product != null)
                {
                    cart2.ItemName = product.Name;
                    cart2.ItemDescription = product.Description;

                }
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
            DateTime wDate;

            var context = new AppDbContext();

            var order = new Order();

            order.OrderDate = DateTime.Now;
            wDate = order.OrderDate;
            order.Total = 0;
            var cId = Session["SessId"] as string;
            order.CartId = cId;
            order.Email = a.Email;
            context.Orders.Add(order);
            var affectedRows = context.SaveChanges();


            // Get order id from Order table

            int wOrderId = 0;
            var orders = context.Orders.ToList().Select(x => new Order 
            {
                CartId = x.CartId,
                OrderId = x.OrderId,
            }).ToList();

            foreach (var item in orders)
            {
               if(item.CartId == cId && item.OrderId > wOrderId)
                {
                        wOrderId = item.OrderId;
                } 
            }


                // --------------------------------------------------          
                // Save OrderId

                if (Session["OrderId"] == null)
                {
                    Session.Add("OrderId", wOrderId);
                }
                // --------------------------------------------------          


            if (wOrderId != 0)
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
                    //  or = context.Orders.FirstOrDefault(x => x.CartId == cId);
                      var or = context.Orders.FirstOrDefault(x => x.OrderId == wOrderId);

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

                // --------------------------------------------------          
                // Save Email address
                // Used as key in Invoice Address table

                if (Session["Email"] == null)
                {
                    string wem = a.Email;
                    Session.Add("Email", wem);
                }
                // --------------------------------------------------          

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
            return RedirectToAction("OrderConfirm", "Shop");

        }




        /// <summary>
        /// Show Order Confirmation
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderConfirm()
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
                return View("OrderConfirm", Carts2);
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

                // Get order total
                int wOrderId = (int)Session["OrderId"];
                // var cId = Session["SessId"] as string;
                // var or = context.Orders.FirstOrDefault(x => x.CartId == cId);
                 var or = context.Orders.FirstOrDefault(x => x.OrderId == wOrderId);

                if (or != null)
                {
                    ViewBag.Total = "Order total: " + or.Total;
                }

                // Get Invoice Address
                var wem = Session["Email"] as string;
                var ia = context.InvoiceAddresses.FirstOrDefault(x => x.Email == wem);

                if (ia != null)
                {
                    ViewBag.Name = "Name: " + ia.FirstName + " " + ia.LastName;
                    ViewBag.City = "City: " + ia.City;
                    ViewBag.Address = "Address: " + ia.PostalCode + " " + ia.Address;
                    ViewBag.Country = "Country: " + ia.Country;

                    ViewBag.Email = "Email: " + ia.Email;
                    ViewBag.PhoneNumber = "Phone number: " + ia.PhoneNumber;
                }



                return View("OrderConfirm", Carts2);

            }

        }

        /// <summary>
        /// Exit Shop
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            var cId = Session["SessId"] as string;
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

            //
            if (Carts == null)
            {
                Session.Remove("SessId");
                Session.Remove("Email");
                Session.Remove("OrderId");
                return RedirectToAction("Index", "Home");
            }
            else
            {

                foreach (var item in Carts)
                {

                    if (item.CartId == Session["SessId"] as string)
                    {
                        // Empty the cart
                        var ca = context.Carts.FirstOrDefault(x => x.CartId == cId);

                        if (ca != null)
                        {
                            context.Carts.Remove(ca);
                            var affectedRows = context.SaveChanges();
                        }
                    }
                }

                Session.Remove("SessId");
                Session.Remove("Email");
                Session.Remove("OrderId");

                return RedirectToAction("Index", "Home");
            }

        }




        /// <summary>
        /// Shopping history
        /// </summary>
        /// <returns></returns>
        public ActionResult ListShopHistory()
        {

            var context = new AppDbContext();
            string wEmail = " ";

            // If logged in, get email from Identity user

            if (User.Identity.IsAuthenticated)
            {
                var usid = User.Identity.GetUserName();
                var us = context.Users.FirstOrDefault(x => x.UserName == usid);

                if (us != null)
                {
                   wEmail = us.Email;
                }
            }

            // Get order details
            var odet  = context.OrderDetails.ToList().Select(x => new OrderDetail 
            {
                OrderDetailId = x.OrderDetailId,
                OrderId = x.OrderId,
                ItemId = x.ItemId,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
            }).ToList();

            List<HistoryVM> History = new List<HistoryVM>();

            if (odet != null)
            {
                foreach (var item in odet)
                {
                    // Get order head
                    var or = context.Orders.FirstOrDefault(x => x.OrderId == item.OrderId);

                    if (or != null && or.Email == wEmail)
                    {
                        // Get item name
                        var it = context.Items.FirstOrDefault(x => x.ItemId == item.ItemId);

                        if(it != null)
                        {
                            var hist = new HistoryVM();
                            hist.OrderId = item.OrderId;
                            hist.ItemName = it.Name;
                            hist.Price = item.UnitPrice;
                            hist.Quantity = item.Quantity;
                            hist.OrderDate = or.OrderDate;
                            History.Add(hist);

                        }

                    }




                }
            }

            // Get Invoice Address
            var ia = context.InvoiceAddresses.FirstOrDefault(x => x.Email == wEmail);

            if (ia != null)
            {
                ViewBag.Name = "Name: " + ia.FirstName + " " + ia.LastName;
                ViewBag.City = "City: " + ia.City;
                ViewBag.Address = "Address: " + ia.PostalCode + " " + ia.Address;
                ViewBag.Country = "Country: " + ia.Country;

                ViewBag.Email = "Email: " + ia.Email;
                ViewBag.PhoneNumber = "Phone number: " + ia.PhoneNumber;
            }

            return View("OrderHistory", History);

        }


        /// <summary>
        /// Exit History
        /// </summary>
        /// <returns></returns>
        public ActionResult ExitHistory()
        {
            return RedirectToAction("Index", "Home");

        }




    }
    }