using Project01.Models;
using Project01.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project01.Controllers
{
    public class HomeController : Controller


    {


        public ActionResult Index()
        {

            //  ViewBag.Title = "Home";
            ViewBag.Message = "A site for enthusiasts of vintage vehicles.";
            ViewBag.Message2 = "Click on Webshop for Manuals, Exploded Views and more...";
            return View();
        }
              
        public ActionResult About()
        {
            ViewBag.Title = "About";
            ViewBag.Message = "Some information";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Name and address";
            return View();
        }

    }
}