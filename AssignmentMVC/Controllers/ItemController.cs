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
    public class ItemController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show picture of available products
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
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


        public ActionResult RenderItem(Item p)
        {
            return PartialView("_item", p);
        }


        /// <summary>
        /// Search products by item.Name
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchItem(string Find)
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

            List<Item> findList = new List<Item>();

            if (Items == null)
            {
                return View("ListItems", findList);
            }
            else
            {
                foreach (var item in Items)
                {
                    if (item.Name != null)
                    {
                        if (item.Name.Contains(Find))
                        {
                            findList.Add(item);
                        }
                    }
                }
                    return View("ListItems", findList);
            }

        }


        /// <summary>
        /// Add a product to the database
        /// </summary>
        /// <param name="model">Item</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddItem(Item model)
        {
            if (ModelState.IsValid)
            {
                var context = new AppDbContext();

                var item = new Item();

                item.Name = model.Name;
                item.Price = model.Price;
                item.Picture = model.Picture;
                item.Description = model.Description;
                item.StockQty = model.StockQty;

                context.Items.Add(item);
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    ViewBag.Message = "Item " + model.Name + " added.";
                    //   return RedirectToAction("Index", "Home");
                       return RedirectToAction("ListItems", "Item");

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
        /// Delete a product from the database
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string find)
        {
            var context = new AppDbContext();
            int affectedRows = 0;
            int wItemId = 0;
            var it = context.Items.FirstOrDefault(x => x.Name == find);

            if (it != null)
            {
                wItemId = it.ItemId;
                context.Items.Remove(it);
                affectedRows = context.SaveChanges();
            }

            if (affectedRows > 0)
            {
                ViewBag.Message = "Item " + find + " deleted.";
                var item = new Item();
                item.ItemId = wItemId; 
                item.Name = "Deleted";
                item.Price = 0;
                item.Picture = " ";
                item.Description = " ";
                item.StockQty = 0;
                return PartialView("_item", item);
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
                var item = new Item();
                item.ItemId = 0;
                item.Name = "Deleted";
                return PartialView("_item", item);
            }

        }


        /// <summary>
        /// Edit a product in the database
        /// </summary>
        /// <param name="p">Item</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ItemId, Name, Price, Picture, Description, StockQty")]Item p)
        {
            var context = new AppDbContext();
            var it = context.Items.FirstOrDefault(x => x.ItemId == p.ItemId);

            if (ModelState.IsValid && it != null)
            {
                it.Name = p.Name;
                it.Price = p.Price;
                it.Picture = p.Picture;
                it.Description = p.Description;
                it.StockQty = p.StockQty;
                var affectedRows = context.SaveChanges();

                return PartialView("_item", p);


            }
            else
            {
                var item = new Item();
                item.ItemId = 0;
                item.Name = "Deleted";
                return PartialView("_item", it);
            }
        }


        /// <summary>
        /// Edit a Item in the database
        /// </summary>
        /// <param name="id">ItemId</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditItem(int id = 0)
        {
            var context = new AppDbContext();

            var item = context.Items.FirstOrDefault(x => x.ItemId == id);
            if (item == null)
            {
                item = new Item();
                item.ItemId = id;
                item.Name = "Deleted";
                return PartialView("_item", item);
            }
            else
            {
                return PartialView("_EditItem", item);
            }
        }



        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var it = context.Items.FirstOrDefault(x => x.ItemId == id);
            if(it == null)
            {
                var item = new Item();
                item.ItemId = id;
                item.Name = "Deleted";
            }
            return PartialView("_item", it);
        }





    }
}