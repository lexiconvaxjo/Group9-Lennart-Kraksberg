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
        // GET: Order detail
        public ActionResult Index()
        {
            return View();
        }


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



        public ActionResult RenderOrderDetail(OrderDetail p)
        {
            return PartialView("_orderDetail", p);
        }




        [HttpPost]
        public ActionResult SearchOrderDetail(string Search)
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

            List<OrderDetail> searchList = new List<OrderDetail>();
            int numSearch = 0;

            if (OrderDetails == null)
            {
                return View("ListOrderDetails", searchList);
            }
            else
            {
                foreach (var item in OrderDetails)
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
                return View("ListOrderDetails", searchList);
            }

        }



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
            var od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == search);

            context.OrderDetails.Remove(od);
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

        [HttpPost]
        public ActionResult EditOrderDetail(int id = 0)
        {
            var context = new AppDbContext();

            var orderDetail = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
            return PartialView("_EditOrderDetail", orderDetail);
        }

        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var od = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
            return PartialView("_orderDetail", od);
        }


    }
}