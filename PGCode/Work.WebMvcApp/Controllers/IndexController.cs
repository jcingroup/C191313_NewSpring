using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProcCore;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using ProcCore.JqueryHelp.JQGridScript;
using DotWeb.CommSetup;

namespace DotWeb.WebApp.Controllers
{
    public class IndexController :  WebFrontController
    {
        public ActionResult Index()
        {
            var qa = new a_News() { Connection = getSQLConnection(), logPlamInfo= plamInfo};
            RunQueryPackage<m_News> r = qa.SearchMaster(new q_News() { s_isopen = true,s_setdate = DateTime.Now }, 1);
            List<m_News> rs = new List<m_News>();
            for (int i = 0; i < 6 && i < r.Count; i++)
                rs.Add(r.SearchData[i]);
            return View(rs);
        }       
    }
}
