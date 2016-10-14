using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProcCore;
using ProcCore.WebCore;
using ProcCore.NetExtension;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.ReturnAjaxResult;
using ProcCore.JqueryHelp.JQGridScript;
using Newtonsoft.Json;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class UsersController : BaseAction<m_Users, a_Users, q_Users, UsersData>
    {
        public override String ajax_MasterDeleteData(string ids)
        {
            throw new NotImplementedException();
        }

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public override ActionResult ListGrid(q_Users sh)
        {
            operationMode = OperationMode.EnterList;
            HandleRequest HRq = new HandleRequest();
            //記錄QueryString
            HRq.encodeURIComponent = true;
            HRq.Remove("page");

            ViewBag.Page = QueryGridPage();
            ViewBag.Caption = GetSystemInfo().prog_name;
            ViewBag.AppendQuertString = HRq.ToQueryString();
            HRq = null;

            return View("ListData", sh);
        }

        public override ActionResult EditMasterNewData()
        {
            ac = new a_Users() { Connection = getSQLConnection() };

            operationMode = OperationMode.EditInsert;
            md = new m_Users();

            //設定預設值
            md.isadmin = false;
            md.id = ac.GetIDX();
            md.EditType = EditModeType.Insert;
            md.type = "N";
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("Id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;

            return View("EditData", md);
        }

        public override ActionResult EditMasterDataByID(int id)
        {
            operationMode = OperationMode.EditModify;
            ac = new a_Users() { Connection = getSQLConnection() };

            RunOneDataEnd<m_Users> HResult = ac.GetDataMaster(id, LoginUserId);
            md = HResult.SearchData;
            md.EditType = EditModeType.Modify;
            HandleResultCheck(HResult);
            HandleCollectDataToOptions();

            ViewBag.Caption = GetSystemInfo().prog_name;

            HandleRequest HRq = new HandleRequest();  //記錄QueryString
            HRq.Remove("id"); //不需記ID
            ViewBag.QueryString = HRq.ToQueryString();
            HRq = null;

            return View("EditData", md);
        }

        public RedirectResult Logout()
        {
            Session.Remove("Id");
            Session.Remove("UnitId");
            return Redirect(Url.Content("~/_SysAdm?t=" + DateTime.Now.Ticks));
        }

        protected override void HandleCollectDataToOptions()
        {
            ViewBag.Unit_Option = MakeCollectDataToOptions(ac.MakeOption_Unit(), false);
            ViewBag.UsersState_Option = MakeCollectDataToOptions(ac.MakeOption_UsersState(), false);
        }

        public ActionResult Password()
        {
            operationMode = OperationMode.EditModify;
            Password md_password = new Password();
            md_password.EditType = EditModeType.Modify;
            md_password.id = LoginUserId;
            return View(md_password);
        }

        #region Ajax Call Section

        [HttpGet]
        public override String ajax_MasterGridData(q_Users queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_Users() { Connection = getSQLConnection() };
            RunQueryPackage<m_Users> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);

            UnitData unit = new UnitData();
            var dic = unit.CollectIdNameFields(x => x.id, x => x.name, getSQLConnection());

            foreach (m_Users md in Modules)
            {
                List<String> setFields = new List<String>(5);

                setFields.Add(md.id.ToString());
                setFields.Add(md.account);
                setFields.Add(md.unit.TableCodeValue(dic));
                setFields.Add(md.state.CodeValue(CodeSheet.使用者狀態.MakeCodes()));
                setFields.Add(md.isadmin.BooleanValue(BooleanSheet.ynv));
                setRowElement.Add(new RowElement() { id = md.id.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料
            JQGridDataObject dataObj = new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount
            };

            return JsonConvert.SerializeObject(dataObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            #endregion
        }

        [HttpPost]
        public String ajax_MasterUpdata(m_Users md)
        {
            ReturnAjaxInfo rAjaxResult = new ReturnAjaxInfo();
            ac = new a_Users() { Connection = getSQLConnection() };

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public String ajax_MasterUpdataPassword(Password md)
        {
            ReturnAjaxInfo rAjaxResult = new ReturnAjaxInfo();
            ac = new a_Users() { Connection=getSQLConnection()};
            //修改
            md.id = LoginUserId;
            RunEnd HResult = ac.UpdateMasterPassword(md, LoginUserId.ToString());
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
            rAjaxResult.id = md.id;
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public String ajax_MasterDeleteData()
        {
            String[] deleteID = Request.Form["id"].Split(',');

            ReturnAjaxInfo rAjaxResult = new ReturnAjaxInfo();
            ac = new a_Users() { Connection = getSQLConnection() };

            RunDeleteEnd HResult = ac.DeleteMaster(deleteID, 0);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return JsonConvert.SerializeObject(rAjaxResult, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        [HttpPost]
        public String ajax_Exist_Account(String account)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_Users();
            ac.Connection = this.getSQLConnection();
            return JsonConvert.SerializeObject(ac.Exists_Account(account, LoginUserId), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            #endregion
        }
        #endregion
    }
}
