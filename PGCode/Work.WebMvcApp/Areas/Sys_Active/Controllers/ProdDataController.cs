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

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class ProdDataController : BaseAction<m_Product, a_Product, q_Product, Product>
    {
        #region Action and function section

        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public override ActionResult ListGrid(q_Product sh)
        {
            operationMode = OperationMode.EnterList;
            HandleRequest HRq = new HandleRequest(); //記錄QueryString            
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
            operationMode = OperationMode.EditInsert;


            ac = new a_Product() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            md = new m_Product() { id = ac.GetIDX(), is_open=true };
            md.EditType = EditModeType.Insert;
            #region 新增欄位預設值設定
            //md.SetDate = DateTime.Now;
            #endregion
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
            ac = new a_Product() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunOneDataEnd<m_Product> HResult = ac.GetDataMaster(id, LoginUserId);
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

        /// <summary>
        /// 介面製作，例如Option Radio CheckBox 多項目
        /// </summary>
        protected override void HandleCollectDataToOptions()
        {
            a_Product_Category_L1 acm = new a_Product_Category_L1() { Connection = getSQLConnection(), logPlamInfo = this.plamInfo };
            ViewBag.options_category_l1 = MakeCollectDataToOptions(acm.SearchMaster(new q_Product_Category_L1(), 0).SearchData.ToDictionary(x => x.id.ToString(), x => x.category_l1_name), false);
            ViewBag.options_category_l2 = MakeCollectDataToOptions(new Dictionary<String, String>(), false);
        }
        #endregion

        #region ajax call section
        /// <summary>
        /// 新增修改提供給Ajax Form Call
        /// </summary>
        /// <returns>JSON format for jquery</returns>
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_MasterUpdata(m_Product md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            ac = new a_Product() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = this.ac.InsertMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Insert_Success);
                rAjaxResult.id = HResult.InsertId;
            }
            else
            {
                //修改
                RunEnd HResult = this.ac.UpdateMaster(md, LoginUserId);
                rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.id = md.id;
            }
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// Grid Ajax 刪除資料 Section
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public override String ajax_MasterDeleteData(String id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            string returnString = string.Empty;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ac = new a_Product() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunDeleteEnd HResult = ac.DeleteMaster(id.Split(',').CInt(), LoginUserId);
            rAjaxResult = HandleResultAjaxFiles(HResult, Resources.Res.Data_Delete_Success);
            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// 由ajax從這裡下載資料 並搭配queryObj作為Search條件搜尋
        /// </summary>
        /// <param name="queryObj"></param>
        /// <returns></returns>
        [HttpGet]
        public override String ajax_MasterGridData(q_Product queryObj)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            ac = new a_Product() { Connection = getSQLConnection(), logPlamInfo = plamInfo }; ;

            RunQueryPackage<m_Product> HResult = ac.SearchMaster(queryObj, LoginUserId);
            HandleResultCheck(HResult);
            #endregion
            #region 設定 Page物件 頁數 總筆數 每頁筆數
            int page = (queryObj.page == null ? 1 : queryObj.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.DefPageSize, HResult.Count);
            #endregion
            #region 每行及每個欄位資料組成
            List<RowElement> setRowElement = new List<RowElement>();
            var Modules = HResult.SearchData.Skip(startRecord).Take(this.DefPageSize);
            foreach (m_Product md in Modules)
            {
                List<String> setFields = new List<String>();

                setFields.Add(md.id.ToString());
                setFields.Add(md.product_serial);
                setFields.Add(md.product_name);
                setFields.Add(md.category_l1_name);
                setFields.Add(md.is_open.BooleanValue(BooleanSheet.ynv));

                setRowElement.Add(new RowElement() { id = md.id.ToString(), cell = setFields.ToArray() });
            }
            #endregion
            #region 回傳JSON資料
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(new JQGridDataObject()
            {
                rows = setRowElement.ToArray(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount
            });
            #endregion
        }
        #endregion

        #region ajax file upload handle
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_UploadFine(int Id, String FilesKind, String hd_FileUp_EL)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            #region
            String tpl_File = String.Empty;
            try
            {
                //判斷是否為圖片檔
                if (!IMGExtDef.Any(x => x == hd_FileUp_EL.GetFileExt().ToLower()))
                {
                        HandFineSave(hd_FileUp_EL, Id, new FilesUpScope(), FilesKind, false);
                        rAjaxResult.result = true;
                        rAjaxResult.success = true;
                        rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                }
                else
                {
                    if (FilesKind == "ListImg")
                    {
                        HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.ProductList, FilesKind);
                        rAjaxResult.result = true;
                        rAjaxResult.success = true;
                        rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                    }

                    if (FilesKind == "ShowImg")
                    {
                        HandImageSave(hd_FileUp_EL, Id, ImageFileUpParm.ProductShow, FilesKind);
                        rAjaxResult.result = true;
                        rAjaxResult.success = true;
                        rAjaxResult.FileName = hd_FileUp_EL.GetFileName();
                    }
                }
            }
            catch (LogicError ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = GetRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = ex.Message;
            }
            #endregion
            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_ListFiles(int Id, String FileKind)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.filesObject = ListSysFiles(Id, FileKind);
            rAjaxResult.result = true;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_DeleteFiles(int Id, String FileKind, String FileName)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            DeleteSysFile(Id, FileKind, FileName, ImageFileUpParm.NewsBasicDouble);
            rAjaxResult.result = true;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }
        #endregion
    }
}