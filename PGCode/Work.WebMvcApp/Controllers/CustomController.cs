﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class CustomController : WebFrontController
    {
        //
        // GET: /Custom/

        public ActionResult Custom()
        {
            ViewBag.BodyClass = "Custom";
            return View();
        }

    }
}
