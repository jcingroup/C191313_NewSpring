using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class MonthController : WebFrontController
    {
        //
        // GET: /Month/

        public ActionResult Month()
        {
            ViewBag.BodyClass = "Products Month";
            return View();
        }

    }
}
