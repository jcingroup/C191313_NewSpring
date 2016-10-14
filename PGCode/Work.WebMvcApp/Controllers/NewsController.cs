using ProcCore.Business.Logic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DotWeb.WebApp.Controllers
{
    public class NewsController : WebFrontController
    {
        //
        // GET: /News/

        public ActionResult News()
        {
            ViewBag.BodyClass = "News";
            var qa = new a_News() {Connection = getSQLConnection(),logPlamInfo=plamInfo };
            RunQueryPackage<m_News> r = qa.SearchMaster(new q_News() { s_isopen = true, s_setdate = DateTime.Now }, 1);
            List<m_News> rs = new List<m_News>();
            for (int i = 0; i < r.Count; i++)
                rs.Add(r.SearchData[i]);
            return View(rs);
        }

    }
}
