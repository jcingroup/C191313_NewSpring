using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class ParamController : BaseController
    {
        #region action and function section
        public RedirectResult Index()
        {
            return Redirect(Url.Action("EditMasterDataByID"));
        }

        public ActionResult EditMasterDataByID()
        {
            _Parm parm = new _Parm() { Connection = getSQLConnection(), logPlamInfo = this.plamInfo };
            return View("EditData", parm);
        }


        [HttpPost]
        [ValidateInput(false)]
        public String ajax_MasterUpdata(Parm md)
        {
            _Parm PARMDataHandle = new _Parm() { Connection = getSQLConnection(),logPlamInfo=this.plamInfo };

            PARMDataHandle.兩件以上運費 = md.兩件以上運費;
            PARMDataHandle.訂單運費_少於 = md.訂單運費_少於;
            PARMDataHandle.訂單運費設定 = md.訂單運費設定;
            PARMDataHandle.產品價格方式 = md.產品價格方式;
            PARMDataHandle.產品價格折扣 = md.產品價格折扣;
            PARMDataHandle.貨到付款手續費 = md.貨到付款手續費;
            PARMDataHandle.單樣產品運費 = md.單樣產品運費;
            PARMDataHandle.需付運費 = md.需付運費;
            PARMDataHandle.轉入帳號 = md.轉入帳號;
            PARMDataHandle.ATM戶名 = md.ATM戶名;
            PARMDataHandle.ATM代碼 = md.ATM代碼;
            PARMDataHandle.ATM銀行 = md.ATM銀行;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.title = Resources.Res.Info_WorkResult;
            rAjaxResult.result = true;
            rAjaxResult.message = "參數更新完成";
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        #endregion


    }
}
