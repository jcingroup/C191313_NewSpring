using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class AboutUsController : WebFrontController
    {
        //
        // GET: /AboutUs/

        public AboutUsController()
        {
            ViewBag.BodyClass = "AboutUs";
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult AboutUs2()
        {
            return View();
        }

        public ActionResult AboutUs3()
        {
            return View();
        }

        public ActionResult AboutUs4()
        {
            return View();
        }

    }
}
