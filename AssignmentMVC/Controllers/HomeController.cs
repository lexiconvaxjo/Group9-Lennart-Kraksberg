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

        /// <summary>
        /// Starting page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            //  ViewBag.Title = "Home";
            ViewBag.Message = "A site for enthusiasts of vintage vehicles.";
            ViewBag.Message2 = "Click on Webshop for Manuals, Exploded Views and more...";
            return View();
        }
         
        
        /// <summary>
        /// Some information
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Title = "About";
            ViewBag.Message = "Web shop project. Lexicon Växjö 2016";

            return View();
        }


        /// <summary>
        /// Contact information
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Title = "Name and address";
            return View();
        }

    }
}