using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class SellController : WebFrontController
    {
        //
        // GET: /Sell/

        public ActionResult Sell()
        {
            ViewBag.BodyClass = "Sell";
            return View();
        }

    }
}
