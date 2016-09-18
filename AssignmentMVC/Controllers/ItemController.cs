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
        // GET: Item
        public ActionResult Index()
        {
            return View();
        }


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


        [HttpPost]
        public ActionResult SearchItem(string Search)
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

            List<Item> searchList = new List<Item>();

            if (Items == null)
            {
                return View("ListItems", searchList);
            }
            else
            {
                foreach (var item in Items)
                {
                    if (item.Name != null)
                    {
                        if (item.Name.Contains(Search))
                        {
                            searchList.Add(item);
                        }
                    }
                }
                    return View("ListItems", searchList);
            }

        }



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



        [HttpPost]
        public ActionResult Delete(string search)
        {
            var context = new AppDbContext();
            var it = context.Items.FirstOrDefault(x => x.Name == search);

            context.Items.Remove(it);
            var affectedRows = context.SaveChanges();

            if (affectedRows > 0)
            {
                ViewBag.Message = "Item " + search + " deleted.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
                return View();
            }

        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "ItemId, Name, Price, Picture, Description, StockQty")]Item p)
        {
            var context = new AppDbContext();
            var it = context.Items.FirstOrDefault(x => x.ItemId == p.ItemId);

            if (ModelState.IsValid)
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
                return PartialView("_item", it);
            }
        }

        [HttpPost]
        public ActionResult EditItem(int id = 0)
        {
            var context = new AppDbContext();

            var item = context.Items.FirstOrDefault(x => x.ItemId == id);
            return PartialView("_EditItem", item);
        }

        [HttpPost]
        public ActionResult Cancel(int id = 0)
        {
            var context = new AppDbContext();
            var it = context.Items.FirstOrDefault(x => x.ItemId == id);
            return PartialView("_item", it);
        }





    }
}