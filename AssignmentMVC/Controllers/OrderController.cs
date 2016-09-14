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
        // GET: Order detail
        public ActionResult Index()
        {
            return View();
        }


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



        public ActionResult RenderOrder(OrderVM p)
        {
            return PartialView("_orderPart", p);
        }




        [HttpPost]
        public ActionResult SearchOrder(string Search)
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

            List<OrderVM> searchList = new List<OrderVM>();
            int numSearch = 0;

            if (Orders == null)
            {
                return View("ListOrders", searchList);
            }
            else
            {
                foreach (var item in Orders)
                {
                    if (Search == "")
                    {
                        searchList.Add(item);
                    }
                    else
                    {
                        numSearch = int.Parse(Search);
                        if (item.OrderId == numSearch)
                            searchList.Add(item);
                    }

                }
                return View("ListOrders", searchList);
            }

        }



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




        public ActionResult Delete(int search)
        {
            var context = new AppDbContext();
            var od = context.Orders.FirstOrDefault(x => x.OrderId == search);

            context.Orders.Remove(od);
            var affectedRows = context.SaveChanges();

            if (affectedRows > 0)
            {
                ViewBag.Message = "Order detail " + search + " deleted.";
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