using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using DotWeb.CommSetup;
using Newtonsoft.Json;
using ProcCore.Business.Logic;
using ProcCore;
using System.Linq;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class SystemLoginController : SourceController
    {
        public ActionResult Index()
        {
            Session["MenuHtmlString"] = null;

            a__Lang ac = new a__Lang()
            {
                Connection = getSQLConnection(),
                logPlamInfo = new ProcCore.Log.LogPlamInfo()
                {
                    UserId = 0,
                    AllowWrite = true,
                    BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version,
                    IP = System.Web.HttpContext.Current.Request.UserHostAddress
                }
            };

            RunQueryPackage<m__Lang> r = ac.SearchMaster(new q__Lang() { s_isuse=true }, 0);

            List<SelectListItem> p = new List<SelectListItem>();

            foreach (var opt in r.SearchData)
                p.Add(new SelectListItem() { Value = opt.lang, Text = opt.area });

            ViewBag.Lang_Option = p;

            return View();
        }

        [HttpPost]
        public String ajax_Login(String account, String password, String img_vildate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            LoginResult getLoginResult = new LoginResult();
            a_Users ac = new a_Users() { Connection = getSQLConnection(), logPlamInfo = new Log.LogPlamInfo() { UserId = 0, IP = System.Web.HttpContext.Current.Request.UserHostAddress, BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version } };
            try
            {
                getLoginResult.vildate = Session["CheckCode"].ToString() != img_vildate ? false : true;

                if (!getLoginResult.vildate)
                    throw new Exception(Resources.Res.Log_Err_ImgValideNotEquel);

                LoginSate CheckLoginState = ac.SystemLogin(account, password);

                if (CheckLoginState.Result)
                {
                    getLoginResult.result = true;
                    getLoginResult.url = Url.Content(CommWebSetup.ManageDefCTR);

                    Session.Timeout = 720;
                    Session["Id"] = CheckLoginState.Id;
                    Session["UnitId"] = CheckLoginState.Unit;

                    ViewData["WebAppPath"] = System.Web.HttpContext.Current.Request.ApplicationPath == "/" ? System.Web.HttpContext.Current.Request.ApplicationPath : System.Web.HttpContext.Current.Request.ApplicationPath + "/";
                    ViewData["user"] = CheckLoginState.Acccount;

                    //登錄記錄
                    
                    a__UserLoginLog ac_UserLoginLog = new a__UserLoginLog() { Connection = getSQLConnection(), logPlamInfo = new Log.LogPlamInfo() { UserId = 0, IP = System.Web.HttpContext.Current.Request.UserHostAddress, BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version } };
                    var r1 = ac_UserLoginLog.SearchMaster(new q__UserLoginLog() { s_account = CheckLoginState.Acccount,MaxRecord=1,sord="desc",sidx="logintime"}, CheckLoginState.Id).SearchData;
                    if (r1.Count() > 0)
                    {
                        ViewData["lastlogin"] = r1.FirstOrDefault().logintime;
                    }
                    else {
                        ViewData["lastlogin"] = DateTime.Now;
                    }

                    m__UserLoginLog md = new m__UserLoginLog() { 
                        ip = System.Web.HttpContext.Current.Request.UserHostAddress ,
                        account = CheckLoginState.Acccount,
                        logintime = DateTime.Now,
                        browers = System.Web.HttpContext.Current.Request.Headers.ToString()
                    };
                    var r3 = ac_UserLoginLog.InsertMaster(md, CheckLoginState.Id);

                    //語系使用
                    HttpCookie WebLang = Request.Cookies[CommWebSetup.WebCookiesId + ".Lang"];
                    a__Lang ac_Lang = new a__Lang() { Connection = getSQLConnection(), logPlamInfo = new Log.LogPlamInfo() { UserId = CheckLoginState.Id, IP = System.Web.HttpContext.Current.Request.UserHostAddress, BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version } };
                    var r2 = ac_Lang.SearchMaster(new q__Lang() { s_lang = WebLang.Value }, CheckLoginState.Id).SearchData.FirstOrDefault();
                    ViewData["lang"] = r2.area;

                    ViewResult resultView = View("Manage/vucMenu");

                    StringResult sr = new StringResult();
                    sr.ViewName = resultView.ViewName;
                    sr.MasterName = resultView.MasterName;
                    sr.ViewData = resultView.ViewData;
                    sr.TempData = resultView.TempData;
                    sr.ExecuteResult(this.ControllerContext);
                    Session["MenuHtmlString"] = sr.ToHtmlString;
                    Log.Write("ajax_Login Login OK!：" + Session["Id"].ToString());
                }
                else
                {
                    getLoginResult.title = Resources.Res.Log_Err_Title;
                    getLoginResult.result = false;
                    getLoginResult.message = GetRecMessage(CheckLoginState.Message);

                    Log.Write("ajax_Login Login Fail!：" + getLoginResult.message);

                    Session.Remove("Id");
                    Session.Remove("UnitId");
                    Session.Remove("MenuHtmlString");
                }
            }
            catch (Exception ex)
            {
                getLoginResult.title = Resources.Res.Log_Err_Title;
                getLoginResult.result = false;
                getLoginResult.message = ex.Message;
            }
            return JsonConvert.SerializeObject(getLoginResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public String ajax_Lang(String lang)
        {
            HttpCookie WebLang = new HttpCookie(CommWebSetup.WebCookiesId + ".Lang", lang);
            Response.Cookies.Add(WebLang);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return JsonConvert.SerializeObject(true, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        class LoginResult
        {
            public String title { get; set; }
            public Boolean vildate { get; set; }
            public Boolean result { get; set; }
            public String message { get; set; }
            public String url { get; set; }
        }
    }
}
