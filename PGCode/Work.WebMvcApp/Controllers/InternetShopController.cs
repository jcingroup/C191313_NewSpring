using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class InternetShopController : WebFrontController
    {
        // GET api/<controller>
        public ActionResult InternetShop()
        {
            ViewBag.BodyClass = "InternetShop";
            return View();
        }
    }
}