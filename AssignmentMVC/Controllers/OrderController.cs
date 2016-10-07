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

            var Orders = context.Orders.ToList().Select(x => new Order
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Total = x.Total,
                CartId = x.CartId,
                Email = x.Email,
            }).ToList();

            return View(Orders);
        }


        /// <summary>
        /// Skapa listan, partial
        /// </summary>
        /// <param name="p">Order</param>
        /// <returns></returns>
        public ActionResult RenderOrder(Order p)
        {
            return PartialView("_order", p);
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

            var Orders = context.Orders.ToList().Select(x => new Order
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Total = x.Total,
                CartId = x.CartId,
                Email = x.Email,
            }).ToList();

            List<Order> findList = new List<Order>();
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
        /// <param name="model">Order</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddOrder(Order model)
        {
            if (ModelState.IsValid)
            {
                var context = new AppDbContext();

                var order = new Order();

                //  order.OrderDate = 
                // order.OrderDate = new DateTime(2016, 09, 12);
                order.OrderDate = DateTime.Now;
                order.Total = model.Total;
                order.Email = model.Email;

                context.Orders.Add(order);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Order added.";
                    //  return RedirectToAction("Index", "Home");
                    return RedirectToAction("ListOrders", "Order");

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
        /// Delete an Order detail from the database
        /// </summary>
        /// <param name="find">OrderId</param>
        /// <returns></returns>
        public ActionResult Delete(int find)
        {
            var context = new AppDbContext();
            var od2 = new Order();
            int affectedRows = 0;
            var od = context.Orders.FirstOrDefault(x => x.OrderId == find);

            if (od != null)
            {
                od2.OrderId = od.OrderId;
              //  od2.OrderDate = 
                od2.Total = 0;
                od2.Email = "DELETED";

                context.Orders.Remove(od);
                affectedRows = context.SaveChanges();
            }

            if (affectedRows > 0)
            {
                ViewBag.Message = "Order detail " + find + " deleted.";
                return PartialView("_order", od2);
                // return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
                // return View();
                return PartialView("_order", od2);
            }

        }







        /// <summary>
        /// Edit an Order detail in the database
        /// </summary>
        /// <param name="p">Order</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OrderId, OrderDate, Total, Email")]Order p)
        {
            var context = new AppDbContext();
            var od = context.Orders.FirstOrDefault(x => x.OrderId == p.OrderId);

            if (ModelState.IsValid && od != null)
            {
                od.OrderDate = p.OrderDate;
                od.Total = p.Total;
                od.Email = p.Email;
                var affectedRows = context.SaveChanges();

                return PartialView("_order", p);


            }
            else
            {
                var od2 = new Order();
                od2.OrderId = p.OrderId;
                return PartialView("_order", od2);
            }
        }



        /// <summary>
        /// Edit an Order in the database
        /// </summary>
        /// <param name="id">OrderId</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrder(int id = 0)
        {
            var context = new AppDbContext();

            var order  = context.Orders.FirstOrDefault(x => x.OrderId == id);

            if (order == null)
            {
                order = new Order();
                order.OrderId = id;
                return PartialView("_order", order);
            }
            else
            {
                return PartialView("_EditOrder", order);
            }
        }


        /// <summary>
        /// Cancel the Edit task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var od = context.Orders.FirstOrDefault(x => x.OrderId == id);
            return PartialView("_order", od);
        }








    }
}