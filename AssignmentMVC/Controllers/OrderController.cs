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
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// Show a list of orders in the database
        /// </summary>
        /// <returns></returns>
        public ActionResult ListOrders()
        {
            var context = new AppDbContext();

            var Orders = context.Orders.ToList().Select(x => new OrderVM
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Country = x.Country,
                City = x.City,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Total = x.Total,
            }).ToList();

            return View(Orders);
        }


        /// <summary>
        /// Skapa listan, partial
        /// </summary>
        /// <param name="p">OrderVM</param>
        /// <returns></returns>
        public ActionResult RenderOrder(OrderVM p)
        {
            return PartialView("_orderPart", p);
        }



        /// <summary>
        /// Search Order by OrderId
        /// </summary>
        /// <param name="Find"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchOrder(string Find)
        {
            var context = new AppDbContext();

            var Orders = context.Orders.ToList().Select(x => new OrderVM
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Country = x.Country,
                City = x.City,
                Address = x.Address,
                PostalCode = x.PostalCode,
                Total = x.Total,
            }).ToList();

            List<OrderVM> findList = new List<OrderVM>();
            int numSearch = 0;

            if (Orders == null)
            {
                return View("ListOrders", findList);
            }
            else
            {
                foreach (var item in Orders)
                {
                    if (Find == "")
                    {
                        findList.Add(item);
                    }
                    else
                    {
                        numSearch = int.Parse(Find);
                        if (item.OrderId == numSearch)
                            findList.Add(item);
                    }

                }
                return View("ListOrders", findList);
            }

        }


        /// <summary>
        /// Add an Order to the database
        /// </summary>
        /// <param name="model">OrderVM</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddOrder(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                var context = new AppDbContext();

                var order = new Order();

                //  order.OrderDate = 
                // order.OrderDate = new DateTime(2016, 09, 12);
                order.OrderDate = DateTime.Now;
                order.UserName = model.UserName;
                order.FirstName = model.FirstName;
                order.LastName = model.LastName;
                order.Email = model.Email;
                order.PhoneNumber = model.PhoneNumber;
                order.Country = model.Country;
                order.City = model.City;
                order.Address = model.Address;
                order.PostalCode = model.PostalCode;
                order.Total = model.Total;

                context.Orders.Add(order);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Order added.";
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



        /// <summary>
        /// Delete an order from the database
        /// </summary>
        /// <param name="find">OrderId</param>
        /// <returns></returns>
        public ActionResult Delete(int find)
        {
            var context = new AppDbContext();
            var od = context.Orders.FirstOrDefault(x => x.OrderId == find);

            context.Orders.Remove(od);
            var affectedRows = context.SaveChanges();

            if (affectedRows > 0)
            {
                ViewBag.Message = "Order detail " + find + " deleted.";
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