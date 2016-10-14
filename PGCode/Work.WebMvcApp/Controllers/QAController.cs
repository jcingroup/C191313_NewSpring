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
    public class QAController : WebFrontController
    {
        //
        // GET: /QA/

        public ActionResult QA()
        {
            ViewBag.BodyClass = "QA";
            var qa = new a_QATable() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_QATable> r = qa.SearchMaster(new q_QATable() { s_isopen = true }, 1);
            List<m_QATable> rs = new List<m_QATable>();
            for (int i = 0; i < r.Count; i++)
                rs.Add(r.SearchData[i]);

            return View(rs);
        }

    }
}
