using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class TrustController : WebFrontController
    {
        //
        // GET: /trust/

        public ActionResult Trust()
        {
            ViewBag.BodyClass = "Trust";
            return View();
        }

    }
}
