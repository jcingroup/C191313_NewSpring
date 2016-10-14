using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;

using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.TableFieldModule;

namespace ProcCore.Business.Logic.TablesDescription
{
    #region DataBase Module

    #region Area

    public class News : TableMap<News>
    {
        public News() { N = "News"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Id.N, this.Id }}; }
        public FieldModule Id = new FieldModule() { M = "Id", N = "ID", T = SQLValueType.Int };
        public FieldModule Title = new FieldModule() { M = "Title", N = "TITLE", T = SQLValueType.String };
        public FieldModule Context = new FieldModule() { M = "Context", N = "CONTEXT", T = SQLValueType.String };
        public FieldModule SetDate = new FieldModule() { M = "SetDate", N = "SETTIME", T = SQLValueType.DateTime };
        public FieldModule IsOpen = new FieldModule() { M = "IsOpen", N = "ISOPEN", T = SQLValueType.Boolean };
    }
    public class QATable : TableMap<QATable>
    {
        public QATable() { N = "QATable"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Id.N, this.Id }}; }
        public FieldModule Id = new FieldModule() { M = "Id", N = "ID", T = SQLValueType.Int };
        public FieldModule Question = new FieldModule() { M = "Question", N = "QUESTION", T = SQLValueType.String };
        public FieldModule Answer = new FieldModule() { M = "Answer", N = "ANSWER", T = SQLValueType.String };
        public FieldModule Sort = new FieldModule() { M = "Sort", N = "SORT", T = SQLValueType.Int };
        public FieldModule IsOpen = new FieldModule() { M = "IsOpen", N = "ISOPEN", T = SQLValueType.Boolean };
    }
    public class TreeData : TableMap<TreeData>
    {
        public TreeData() { N = "TreeData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule deep = new FieldModule() { M = "deep", N = "deep", T = SQLValueType.Int };
        public FieldModule parentId = new FieldModule() { M = "parentId", N = "parentId", T = SQLValueType.Int };
        public FieldModule isfolder = new FieldModule() { M = "isfolder", N = "isfolder", T = SQLValueType.Boolean };
        public FieldModule link = new FieldModule() { M = "link", N = "link", T = SQLValueType.String };
        public FieldModule context = new FieldModule() { M = "context", N = "context", T = SQLValueType.String };
        public FieldModule tabs_1 = new FieldModule() { M = "tabs_1", N = "tabs_1", T = SQLValueType.String };
        public FieldModule tabs_2 = new FieldModule() { M = "tabs_2", N = "tabs_2", T = SQLValueType.String };
        public FieldModule tabs_3 = new FieldModule() { M = "tabs_3", N = "tabs_3", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class ProgData : TableMap<ProgData>
    {
        public ProgData() { N = "ProgData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule area = new FieldModule() { M = "area", N = "area", T = SQLValueType.String };
        public FieldModule controller = new FieldModule() { M = "controller", N = "controller", T = SQLValueType.String };
        public FieldModule action = new FieldModule() { M = "action", N = "action", T = SQLValueType.String };
        public FieldModule path = new FieldModule() { M = "path", N = "path", T = SQLValueType.String };
        public FieldModule page = new FieldModule() { M = "page", N = "page", T = SQLValueType.String };
        public FieldModule prog_name = new FieldModule() { M = "prog_name", N = "prog_name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.String };
        public FieldModule isfolder = new FieldModule() { M = "isfolder", N = "isfolder", T = SQLValueType.Boolean };
        public FieldModule ishidden = new FieldModule() { M = "ishidden", N = "ishidden", T = SQLValueType.Boolean };
        public FieldModule isRoute = new FieldModule() { M = "isRoute", N = "isRoute", T = SQLValueType.Boolean };
        public FieldModule ismb = new FieldModule() { M = "ismb", N = "ismb", T = SQLValueType.Boolean };
        public FieldModule power_serial = new FieldModule() { M = "power_serial", N = "power_serial", T = SQLValueType.Int };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class NewsData : TableMap<NewsData>
    {
        public NewsData() { N = "NewsData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule Title = new FieldModule() { M = "Title", N = "Title", T = SQLValueType.String };
        public FieldModule SetDate = new FieldModule() { M = "SetDate", N = "SetDate", T = SQLValueType.DateTime };
        public FieldModule IsOpen = new FieldModule() { M = "IsOpen", N = "IsOpen", T = SQLValueType.Boolean };
        public FieldModule NewsKind = new FieldModule() { M = "NewsKind", N = "NewsKind", T = SQLValueType.String };
        public FieldModule Context = new FieldModule() { M = "Context", N = "Context", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _WebVisitData : TableMap<_WebVisitData>
    {
        public _WebVisitData() { N = "_WebVisitData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule ip = new FieldModule() { M = "ip", N = "ip", T = SQLValueType.String };
        public FieldModule setdate = new FieldModule() { M = "setdate", N = "setdate", T = SQLValueType.DateTime };
        public FieldModule browser = new FieldModule() { M = "browser", N = "browser", T = SQLValueType.String };
        public FieldModule source = new FieldModule() { M = "source", N = "source", T = SQLValueType.String };
        public FieldModule page = new FieldModule() { M = "page", N = "page", T = SQLValueType.String };
    }
    public class _WebCount : TableMap<_WebCount>
    {
        public _WebCount() { N = "_WebCount"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Cnt.N, this.Cnt }}; }
        public FieldModule Cnt = new FieldModule() { M = "Cnt", N = "Cnt", T = SQLValueType.Int };
    }
    public class _PowerUsers : TableMap<_PowerUsers>
    {
        public _PowerUsers() { N = "_PowerUsers"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ProgID.N, this.ProgID }, { this.UserID.N, this.UserID }, { this.PowerID.N, this.PowerID }}; }
        public FieldModule ProgID = new FieldModule() { M = "ProgID", N = "ProgID", T = SQLValueType.Int };
        public FieldModule UserID = new FieldModule() { M = "UserID", N = "UserID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { M = "PowerID", N = "PowerID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { M = "UnitID", N = "UnitID", T = SQLValueType.Int };
    }

    public class _UserLoginLog : TableMap<_UserLoginLog>
    {
        public _UserLoginLog() { N = "_UserLoginLog"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule account = new FieldModule() { M = "account", N = "account", T = SQLValueType.String };
        public FieldModule ip = new FieldModule() { M = "ip", N = "ip", T = SQLValueType.String };
        public FieldModule logintime = new FieldModule() { M = "logintime", N = "logintime", T = SQLValueType.DateTime };
        public FieldModule browers = new FieldModule() { M = "browers", N = "browers", T = SQLValueType.String };
    }

    public class Product : TableMap<Product>
    {
        public Product()
        {
            N = "Product"; GetTabObj = this;

            this.id = new FieldModule() { M = "product_id", N = "product_id", T = SQLValueType.Int, B = this.N };
            this.product_serial = new FieldModule() { M = "product_serial", N = "product_serial", T = SQLValueType.String, B = this.N };
            this.product_name = new FieldModule() { M = "product_name", N = "product_name", T = SQLValueType.String, B = this.N };
            this.product_intro = new FieldModule() { M = "product_intro", N = "product_intro", T = SQLValueType.String, B = this.N };
            this.product_category_l1_id = new FieldModule() { M = "product_category_l1_id", N = "product_category_l1_id", T = SQLValueType.Int, B = this.N };
            this.product_category_l2_id = new FieldModule() { M = "product_category_l2_id", N = "product_category_l2_id", T = SQLValueType.Int, B = this.N };
            this.edit_memo = new FieldModule() { M = "edit_memo", N = "memo", T = SQLValueType.String, B = this.N };
            this.is_open = new FieldModule() { M = "is_open", N = "is_open", T = SQLValueType.Boolean, B = this.N };
            this.sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int, B = this.N };
            this.price = new FieldModule() { M = "price", N = "price", T = SQLValueType.Int, B = this.N };
            this._隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean, B = this.N };
            this._新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int, B = this.N };
            this._新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int, B = this.N };
            this._新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime, B = this.N };
            this._修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int, B = this.N };
            this._修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int, B = this.N };
            this._修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime, B = this.N };
            this._語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String, B = this.N };

            KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id } };
        }
        public FieldModule id { get; set; }
        public FieldModule product_name { get; set; }
        public FieldModule product_intro { get; set; }
        public FieldModule product_serial { get; set; }
        public FieldModule product_category_l1_id { get; set; }
        public FieldModule product_category_l2_id { get; set; }
        public FieldModule edit_memo { get; set; }
        public FieldModule is_open { get; set; }
        public FieldModule sort { get; set; }
        public FieldModule price { get; set; }
        public FieldModule _隱藏 { get; set; }
        public FieldModule _新增人員 { get; set; }
        public FieldModule _新增單位 { get; set; }
        public FieldModule _新增日期 { get; set; }
        public FieldModule _修改人員 { get; set; }
        public FieldModule _修改單位 { get; set; }
        public FieldModule _修改日期 { get; set; }
        public FieldModule _語系 { get; set; }
    }
    public class Product_Category_L1 : TableMap<Product_Category_L1>
    {
        public Product_Category_L1()
        {
            N = "Product_Category_L1"; GetTabObj = this;
            this.id = new FieldModule() { M = "id", N = "product_category_l1_id", T = SQLValueType.Int, B = this.N };
            this.category_l1_name = new FieldModule() { M = "category_l1_name", N = "category_l1_name", T = SQLValueType.String, B = this.N };
            this.sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int, B = this.N };
            this._隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean, B = this.N };
            this._新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int, B = this.N };
            this._新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int, B = this.N };
            this._新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime, B = this.N };
            this._修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int, B = this.N };
            this._修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int, B = this.N };
            this._修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime, B = this.N };
            this._語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String, B = this.N };

            ; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id } };
        }
        public FieldModule id { get; set; }
        public FieldModule category_l1_name { get; set; }
        public FieldModule sort { get; set; }
        public FieldModule _隱藏 { get; set; }
        public FieldModule _新增人員 { get; set; }
        public FieldModule _新增單位 { get; set; }
        public FieldModule _新增日期 { get; set; }
        public FieldModule _修改人員 { get; set; }
        public FieldModule _修改單位 { get; set; }
        public FieldModule _修改日期 { get; set; }
        public FieldModule _語系 { get; set; }
    }
    public class Product_Category_L2 : TableMap<Product_Category_L2>
    {
        public Product_Category_L2()
        {
            N = "Product_Category_L2"; GetTabObj = this;
            this.id = new FieldModule() { M = "id", N = "product_category_l2_id", T = SQLValueType.Int, B = this.N };
            this.product_category_l1_id = new FieldModule() { M = "product_category_l1_id", N = "product_category_l1_id", T = SQLValueType.Int, B = this.N };
            this.category_l2_name = new FieldModule() { M = "category_l2_name", N = "category_l2_name", T = SQLValueType.String, B = this.N };
            this.sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int, B = this.N };
            this._隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean, B = this.N };
            this._新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int, B = this.N };
            this._新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int, B = this.N };
            this._新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime, B = this.N };
            this._修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int, B = this.N };
            this._修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int, B = this.N };
            this._修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime, B = this.N };
            this._語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String, B = this.N };

            ; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id } };
        }
        public FieldModule id { get; set; }
        public FieldModule product_category_l1_id { get; set; }
        public FieldModule category_l2_name { get; set; }
        public FieldModule sort { get; set; }
        public FieldModule _隱藏 { get; set; }
        public FieldModule _新增人員 { get; set; }
        public FieldModule _新增單位 { get; set; }
        public FieldModule _新增日期 { get; set; }
        public FieldModule _修改人員 { get; set; }
        public FieldModule _修改單位 { get; set; }
        public FieldModule _修改日期 { get; set; }
        public FieldModule _語系 { get; set; }
    }

    public class _PowerUnit : TableMap<_PowerUnit>
    {
        public _PowerUnit() { N = "_PowerUnit"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ProgID.N, this.ProgID }, { this.UnitID.N, this.UnitID }, { this.PowerID.N, this.PowerID }}; }
        public FieldModule ProgID = new FieldModule() { M = "ProgID", N = "ProgID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { M = "UnitID", N = "UnitID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { M = "PowerID", N = "PowerID", T = SQLValueType.Int };
        public FieldModule AccessUnit = new FieldModule() { M = "AccessUnit", N = "AccessUnit", T = SQLValueType.Int };
    }
    public class _PowerName : TableMap<_PowerName>
    {
        public _PowerName() { N = "_PowerName"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { M = "memo", N = "memo", T = SQLValueType.String };
    }
    public class _ParmString : TableMap<_ParmString>
    {
        public _ParmString() { N = "_ParmString"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.String };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmInt : TableMap<_ParmInt>
    {
        public _ParmInt() { N = "_ParmInt"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Int };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmFloat : TableMap<_ParmFloat>
    {
        public _ParmFloat() { N = "_ParmFloat"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Int };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmDateTime : TableMap<_ParmDateTime>
    {
        public _ParmDateTime() { N = "_ParmDateTime"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.DateTime };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _ParmBit : TableMap<_ParmBit>
    {
        public _ParmBit() { N = "_ParmBit"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }}; }
        public FieldModule ParmName = new FieldModule() { M = "ParmName", N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.String };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class _Lang : TableMap<_Lang>
    {
        public _Lang() { N = "_Lang"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.lang.N, this.lang }}; }
        public FieldModule lang = new FieldModule() { M = "lang", N = "lang", T = SQLValueType.String };
        public FieldModule area = new FieldModule() { M = "area", N = "area", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { M = "memo", N = "memo", T = SQLValueType.String };
        public FieldModule isuse = new FieldModule() { M = "isuse", N = "isuse", T = SQLValueType.Boolean };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
    }
    public class _IDX : TableMap<_IDX>
    {
        public _IDX() { N = "_IDX"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.IDX.N, this.IDX }}; }
        public FieldModule IDX = new FieldModule() { M = "IDX", N = "IDX", T = SQLValueType.Int };
    }
    public class _CodeSheet : TableMap<_CodeSheet>
    {
        public _CodeSheet() { N = "_CodeSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.CodeHeadId.N, this.CodeHeadId }, { this.Code.N, this.Code }, { this._語系.N, this._語系 }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule CodeHeadId = new FieldModule() { M = "CodeHeadId", N = "CodeHeadId", T = SQLValueType.Int };
        public FieldModule Code = new FieldModule() { M = "Code", N = "Code", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { M = "Value", N = "Value", T = SQLValueType.String };
        public FieldModule Sort = new FieldModule() { M = "Sort", N = "Sort", T = SQLValueType.Int };
        public FieldModule IsUse = new FieldModule() { M = "IsUse", N = "IsUse", T = SQLValueType.Boolean };
        public FieldModule IsEdit = new FieldModule() { M = "IsEdit", N = "IsEdit", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _CodeHead : TableMap<_CodeHead>
    {
        public _CodeHead() { N = "_CodeHead"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule IsEdit = new FieldModule() { M = "IsEdit", N = "IsEdit", T = SQLValueType.Boolean };
        public FieldModule Memo = new FieldModule() { M = "Memo", N = "Memo", T = SQLValueType.String };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _BooleanSheet : TableMap<_BooleanSheet>
    {
        public _BooleanSheet() { N = "_BooleanSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Boolean.N, this.Boolean }}; }
        public FieldModule Boolean = new FieldModule() { M = "Boolean", N = "Boolean", T = SQLValueType.Boolean };
        public FieldModule sex = new FieldModule() { M = "sex", N = "sex", T = SQLValueType.String };
        public FieldModule yn = new FieldModule() { M = "yn", N = "yn", T = SQLValueType.String };
        public FieldModule ynv = new FieldModule() { M = "ynv", N = "ynv", T = SQLValueType.String };
        public FieldModule ynvx = new FieldModule() { M = "ynvx", N = "ynvx", T = SQLValueType.String };
    }
    public class _AddressCounty : TableMap<_AddressCounty>
    {
        public _AddressCounty() { N = "_AddressCounty"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }, { this.county.N, this.county }}; }
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule county = new FieldModule() { M = "county", N = "county", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { M = "zip", N = "zip", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule code = new FieldModule() { M = "code", N = "code", T = SQLValueType.String };
    }
    public class _AddressCity : TableMap<_AddressCity>
    {
        public _AddressCity() { N = "_AddressCity"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }}; }
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
    }
    ///<summary>
    ///消息資料檔
    ///</summary>
    public class Message : TableMap<Message>
    {
        public Message() { N = "消息"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule title = new FieldModule() { M = "title", N = "標題", T = SQLValueType.String };
        public FieldModule set_date = new FieldModule() { M = "set_date", N = "日期", T = SQLValueType.DateTime };
        public FieldModule context = new FieldModule() { M = "context", N = "內容", T = SQLValueType.String };
        public FieldModule isopen = new FieldModule() { M = "isopen", N = "開放", T = SQLValueType.Boolean };
        public FieldModule kind = new FieldModule() { M = "kind", N = "分類", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    ///<summary>
    ///會員資料檔
    ///</summary>
    public class Member : TableMap<Member>
    {
        public Member() { N = "會員"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule email = new FieldModule() { M = "email", N = "電子郵件", T = SQLValueType.String };
        public FieldModule password = new FieldModule() { M = "password", N = "密碼", T = SQLValueType.String };
        public FieldModule name = new FieldModule() { M = "name", N = "姓名", T = SQLValueType.String };
        public FieldModule tel = new FieldModule() { M = "tel", N = "聯絡電話", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { M = "zip", N = "郵遞區號", T = SQLValueType.String };
        public FieldModule address = new FieldModule() { M = "address", N = "地址", T = SQLValueType.String };
        ///<summary>
        ///email 認證是否有效，預設為false。
        ///</summary>
        public FieldModule IsValidate = new FieldModule() { M = "IsValidate", N = "認證", T = SQLValueType.Boolean };
        public FieldModule pid = new FieldModule() { M = "pid", N = "證號", T = SQLValueType.String };
        public FieldModule sex = new FieldModule() { M = "sex", N = "性別", T = SQLValueType.Boolean };
        public FieldModule birthday = new FieldModule() { M = "birthday", N = "生日", T = SQLValueType.DateTime };
        public FieldModule member_state = new FieldModule() { M = "member_state", N = "狀態", T = SQLValueType.String };
        public FieldModule member_level = new FieldModule() { M = "member_level", N = "等級", T = SQLValueType.Int };
        public FieldModule point = new FieldModule() { M = "point", N = "績點", T = SQLValueType.Int };
        public FieldModule reg_time = new FieldModule() { M = "reg_time", N = "註冊時間", T = SQLValueType.DateTime };
        public FieldModule login_time = new FieldModule() { M = "login_time", N = "登錄時間", T = SQLValueType.DateTime };
        public FieldModule isgroup = new FieldModule() { M = "isgroup", N = "團購身分", T = SQLValueType.Boolean };
        public FieldModule groupunit = new FieldModule() { M = "groupunit", N = "團購單位", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class _Currency : TableMap<_Currency>
    {
        public _Currency() { N = "_Currency"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name_currency = new FieldModule() { M = "name_currency", N = "name_currency", T = SQLValueType.String };
        public FieldModule english_currency = new FieldModule() { M = "english_currency", N = "english_currency", T = SQLValueType.String };
        public FieldModule sign = new FieldModule() { M = "sign", N = "sign", T = SQLValueType.String };
        public FieldModule code = new FieldModule() { M = "code", N = "code", T = SQLValueType.String };
    }
    ///<summary>
    ///訂單主檔
    ///</summary>
    public class OrderM : TableMap<OrderM>
    {
        public OrderM() { N = "訂單M"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule sn_order = new FieldModule() { M = "sn_order", N = "訂單編號", T = SQLValueType.String };
        public FieldModule set_date = new FieldModule() { M = "set_date", N = "交易日期", T = SQLValueType.DateTime };
        public FieldModule fee_trade = new FieldModule() { M = "fee_trade", N = "交易金額", T = SQLValueType.Int };
        public FieldModule fee_shipping = new FieldModule() { M = "fee_shipping", N = "運費", T = SQLValueType.Int };
        public FieldModule fee_total = new FieldModule() { M = "fee_total", N = "總計金額", T = SQLValueType.Int };
        public FieldModule receiver_name = new FieldModule() { M = "receiver_name", N = "收件人", T = SQLValueType.String };
        public FieldModule receiver_sex = new FieldModule() { M = "receiver_sex", N = "收件人_性別", T = SQLValueType.Boolean };
        public FieldModule receiver_zip = new FieldModule() { M = "receiver_zip", N = "收件人_Zip", T = SQLValueType.String };
        public FieldModule receiver_address = new FieldModule() { M = "receiver_address", N = "收件人_地址", T = SQLValueType.String };
        public FieldModule receiver_email = new FieldModule() { M = "receiver_email", N = "收件人_email", T = SQLValueType.String };
        public FieldModule receiver_tel = new FieldModule() { M = "receiver_tel", N = "收件人_電話", T = SQLValueType.String };
        public FieldModule order_name = new FieldModule() { M = "order_name", N = "訂購人", T = SQLValueType.String };
        public FieldModule order_sex = new FieldModule() { M = "order_sex", N = "訂購人_性別", T = SQLValueType.Boolean };
        public FieldModule order_zip = new FieldModule() { M = "order_zip", N = "訂購人_Zip", T = SQLValueType.String };
        public FieldModule order_address = new FieldModule() { M = "order_address", N = "訂購人_地址", T = SQLValueType.String };
        public FieldModule order_tel = new FieldModule() { M = "order_tel", N = "訂購人_電話", T = SQLValueType.String };
        public FieldModule order_email = new FieldModule() { M = "order_email", N = "訂購人_email", T = SQLValueType.String };
        ///<summary>
        ///統一編號
        ///</summary>
        public FieldModule sno = new FieldModule() { M = "sno", N = "統編", T = SQLValueType.String };
        public FieldModule pay_inform = new FieldModule() { M = "pay_inform", N = "付款通知", T = SQLValueType.Boolean };
        public FieldModule pay_date = new FieldModule() { M = "pay_date", N = "付款日期", T = SQLValueType.DateTime };
        ///<summary>
        ///例填入轉帳帳號後5碼
        ///</summary>
        public FieldModule pay_valide = new FieldModule() { M = "pay_valide", N = "付款驗證", T = SQLValueType.String };
        public FieldModule pay_memo = new FieldModule() { M = "pay_memo", N = "付款備註", T = SQLValueType.String };
        public FieldModule pay_style = new FieldModule() { M = "pay_style", N = "付款方式", T = SQLValueType.String };
        public FieldModule pay_state = new FieldModule() { M = "pay_state", N = "付款狀態", T = SQLValueType.String };
        public FieldModule pay_money = new FieldModule() { M = "pay_money", N = "付款金額", T = SQLValueType.Int };
        public FieldModule return_date = new FieldModule() { M = "return_date", N = "退貨日期", T = SQLValueType.DateTime };
        public FieldModule return_reason = new FieldModule() { M = "return_reason", N = "退貨原因", T = SQLValueType.String };
        public FieldModule state = new FieldModule() { M = "state", N = "狀態", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { M = "memo", N = "備註", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    ///<summary>
    ///訂單明細檔
    ///</summary>
    public class OrderS : TableMap<OrderS>
    {
        public OrderS() { N = "訂單S"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ids.N, this.ids }}; }
        public FieldModule ids = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule item = new FieldModule() { M = "item", N = "項次", T = SQLValueType.Int };
        public FieldModule sn_order = new FieldModule() { M = "sn_order", N = "訂單編號", T = SQLValueType.String };
        public FieldModule sn_product = new FieldModule() { M = "sn_product", N = "產品編號", T = SQLValueType.String };
        public FieldModule amt = new FieldModule() { M = "amt", N = "數量", T = SQLValueType.Int };
        public FieldModule unit_price = new FieldModule() { M = "unit_price", N = "單價", T = SQLValueType.Int };
        public FieldModule subcount = new FieldModule() { M = "subcount", N = "小計", T = SQLValueType.Int };
        public FieldModule product_type = new FieldModule() { M = "product_type", N = "型號", T = SQLValueType.String };
        public FieldModule product_standard = new FieldModule() { M = "product_standard", N = "規格", T = SQLValueType.String };
        public FieldModule unit_name = new FieldModule() { M = "unit_name", N = "計價單位", T = SQLValueType.String };
        public FieldModule currency = new FieldModule() { M = "currency", N = "幣別", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    public class UsersData : TableMap<UsersData>
    {
        public UsersData() { N = "UsersData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule account = new FieldModule() { M = "account", N = "account", T = SQLValueType.String };
        public FieldModule password = new FieldModule() { M = "password", N = "password", T = SQLValueType.String };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule unit = new FieldModule() { M = "unit", N = "unit", T = SQLValueType.Int };
        public FieldModule state = new FieldModule() { M = "state", N = "state", T = SQLValueType.String };
        public FieldModule isadmin = new FieldModule() { M = "isadmin", N = "isadmin", T = SQLValueType.Boolean };
        public FieldModule type = new FieldModule() { M = "type", N = "type", T = SQLValueType.String };
        public FieldModule email = new FieldModule() { M = "email", N = "email", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { M = "zip", N = "zip", T = SQLValueType.String };
        public FieldModule city = new FieldModule() { M = "city", N = "city", T = SQLValueType.String };
        public FieldModule country = new FieldModule() { M = "country", N = "country", T = SQLValueType.String };
        public FieldModule address = new FieldModule() { M = "address", N = "address", T = SQLValueType.String };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    public class UnitData : TableMap<UnitData>
    {
        public UnitData() { N = "UnitData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { M = "name", N = "name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "sort", T = SQLValueType.Int };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
    }
    ///<summary>
    ///網頁編輯
    ///</summary>
    public class PageContext : TableMap<PageContext>
    {
        public PageContext() { N = "網頁"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule kid = new FieldModule() { M = "kid", N = "kid", T = SQLValueType.Int };
        public FieldModule pagename = new FieldModule() { M = "pagename", N = "網頁名稱", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "排序", T = SQLValueType.Int };
        public FieldModule pagecontext = new FieldModule() { M = "pagecontext", N = "網頁內容", T = SQLValueType.String };
        public FieldModule setdate = new FieldModule() { M = "setdate", N = "時間", T = SQLValueType.DateTime };
        public FieldModule isopen = new FieldModule() { M = "isopen", N = "開啟", T = SQLValueType.Boolean };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    ///<summary>
    ///產品上架關聯表
    ///</summary>
    public class OnShelf : TableMap<OnShelf>
    {
        public OnShelf() { N = "產品上架"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id_Product.N, this.id_Product }, { this.id_OnShelf.N, this.id_OnShelf }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule id_Product = new FieldModule() { M = "id_Product", N = "id_Product", T = SQLValueType.Int };
        public FieldModule id_OnShelf = new FieldModule() { M = "id_OnShelf", N = "id_OnShelf", T = SQLValueType.Int };
        public FieldModule isopen = new FieldModule() { M = "isopen", N = "開啟", T = SQLValueType.Boolean };
        public FieldModule startdate = new FieldModule() { M = "startdate", N = "啟始日", T = SQLValueType.DateTime };
        public FieldModule enddate = new FieldModule() { M = "enddate", N = "結束日", T = SQLValueType.DateTime };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    ///<summary>
    ///產品上架關聯表歷史記錄
    ///</summary>
    public class OnShelfHistory : TableMap<OnShelfHistory>
    {
        public OnShelfHistory() { N = "產品上架H"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule id_Product = new FieldModule() { M = "id_Product", N = "id_Product", T = SQLValueType.Int };
        public FieldModule id_OnShelf = new FieldModule() { M = "id_OnShelf", N = "id_OnShelf", T = SQLValueType.Int };
        public FieldModule isopen = new FieldModule() { M = "isopen", N = "開啟", T = SQLValueType.Boolean };
        public FieldModule startdate = new FieldModule() { M = "startdate", N = "啟始日", T = SQLValueType.DateTime };
        public FieldModule enddate = new FieldModule() { M = "enddate", N = "結束日", T = SQLValueType.DateTime };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    ///<summary>
    ///上架樹狀分類
    ///</summary>
    public class TreeShelf : TableMap<TreeShelf>
    {
        public TreeShelf() { N = "T上架"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { M = "id", N = "id", T = SQLValueType.Int };
        public FieldModule idp = new FieldModule() { M = "idp", N = "idp", T = SQLValueType.Int };
        public FieldModule shelfname = new FieldModule() { M = "shelfname", N = "上架名稱", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { M = "sort", N = "排序", T = SQLValueType.Int };
        public FieldModule isuse = new FieldModule() { M = "isuse", N = "使用", T = SQLValueType.Boolean };
        public FieldModule isfolder = new FieldModule() { M = "isfolder", N = "目錄", T = SQLValueType.Boolean };
        public FieldModule _異動 = new FieldModule() { M = "_異動", N = "_異動", T = SQLValueType.Boolean };
        public FieldModule _隱藏 = new FieldModule() { M = "_隱藏", N = "_隱藏", T = SQLValueType.Boolean };
        public FieldModule _新增人員 = new FieldModule() { M = "_新增人員", N = "_新增人員", T = SQLValueType.Int };
        public FieldModule _新增單位 = new FieldModule() { M = "_新增單位", N = "_新增單位", T = SQLValueType.Int };
        public FieldModule _新增日期 = new FieldModule() { M = "_新增日期", N = "_新增日期", T = SQLValueType.DateTime };
        public FieldModule _修改人員 = new FieldModule() { M = "_修改人員", N = "_修改人員", T = SQLValueType.Int };
        public FieldModule _修改單位 = new FieldModule() { M = "_修改單位", N = "_修改單位", T = SQLValueType.Int };
        public FieldModule _修改日期 = new FieldModule() { M = "_修改日期", N = "_修改日期", T = SQLValueType.DateTime };
        public FieldModule _語系 = new FieldModule() { M = "_語系", N = "_語系", T = SQLValueType.String };
    }
    #endregion

    #endregion
}
