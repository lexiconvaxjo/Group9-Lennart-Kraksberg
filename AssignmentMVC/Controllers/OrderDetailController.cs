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
    public class OrderDetailController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// Show a list of the Order details
        /// </summary>
        /// <returns></returns>
        public ActionResult ListOrderDetails()
        {
            var context = new AppDbContext();

            var OrderDetails = context.OrderDetails.ToList().Select(x => new OrderDetail
            {
                OrderDetailId = x.OrderDetailId,
                OrderId = x.OrderId,
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList();

            return View(OrderDetails);
        }


        /// <summary>
        /// Show the list of Order details, partial view
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult RenderOrderDetail(OrderDetail p)
        {
            return PartialView("_orderDetail", p);
        }



        /// <summary>
        /// Searc th Order detail table, by OrderId
        /// </summary>
        /// <param name="Find"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchOrderDetail(string Find)
        {
            var context = new AppDbContext();

            var OrderDetails = context.OrderDetails.ToList().Select(x => new OrderDetail
            {
                OrderDetailId = x.OrderDetailId,
                OrderId = x.OrderId,
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList();

            List<OrderDetail> findList = new List<OrderDetail>();
            int numSearch = 0;

            if (OrderDetails == null)
            {
                return View("ListOrderDetails", findList);
            }
            else
            {
                foreach (var item in OrderDetails)
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
                return View("ListOrderDetails", findList);
            }

        }


        /// <summary>
        /// Add an Order detail to the database
        /// </summary>
        /// <param name="model">OrderDetail</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddOrderDetail(OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                var context = new AppDbContext();

                var orderDetail = new OrderDetail();

                orderDetail.OrderId = model.OrderId;
                orderDetail.ItemId = model.ItemId;
                orderDetail.Quantity = model.Quantity;
                orderDetail.UnitPrice = model.UnitPrice;

                context.OrderDetails.Add(orderDetail);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Order detail added.";
                    //  return RedirectToAction("Index", "Home");
                      return RedirectToAction("ListOrderDetails", "OrderDetail");

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
        /// <param name="find">OrderDetailId</param>
        /// <returns></returns>
        public ActionResult Delete(int find)
        {
            var context = new AppDbContext();
            var od2 = new OrderDetail();
            int affectedRows = 0;
            var od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == find);

            if (od != null)
            {
                od2.OrderDetailId = od.OrderDetailId;
                od2.OrderId = 0;
                od2.ItemId = 0;
                od2.Quantity = 0;
                od2.UnitPrice = 0;

                context.OrderDetails.Remove(od);
                affectedRows = context.SaveChanges();
            }

            if (affectedRows > 0)
            {
                ViewBag.Message = "Order detail " + find + " deleted.";
                return PartialView("_orderDetail", od2);
                // return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
                // return View();
                return PartialView("_orderDetail", od2);
            }

        }


        /// <summary>
        /// Edit an Order detail in the database
        /// </summary>
        /// <param name="p">OrderDetail</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OrderDetailId, OrderId, ItemId, Quantity, UnitPrice")]OrderDetail p)
        {
            var context = new AppDbContext();
            var od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == p.OrderDetailId);

            if (ModelState.IsValid)
            {
                od.OrderId = p.OrderId;
                od.ItemId = p.ItemId;
                od.Quantity = p.Quantity;
                od.UnitPrice = p.UnitPrice;
                var affectedRows = context.SaveChanges();

                return PartialView("_orderDetail", p);


            }
            else
            {
                return PartialView("_orderDetail", od);
            }
        }



        /// <summary>
        /// Edit an Order detail in the database
        /// </summary>
        /// <param name="id">OrderDetailId</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditOrderDetail(int id = 0)
        {
            var context = new AppDbContext();

            var orderDetail = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
            return PartialView("_EditOrderDetail", orderDetail);
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
            var od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
            return PartialView("_orderDetail", od);
        }


    }
}