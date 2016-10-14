using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcCore.Business.Logic;
using DotWeb;


namespace DotWeb.WebApp.Controllers
{
    public class ProductsController : WebFrontController
    {
        //
        // GET: /Product/
        WebInfo webInfo = new WebInfo();

        public ProductsController()
        {
            ViewBag.BodyClass = "Products";
        }

        public ActionResult Prolist(int? id, int? ids)
        {

            if (id == null)
                id = 1;

            //if (ids == null)
            //    ids = 1;

            a_Product ac_Product = new a_Product() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };

            ViewBag.Product_Category_Now = id;

            if (id != null && ids == null)
            {
                a_Product_Category_L1 ac_Product_Category_L1 = new a_Product_Category_L1() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
                webInfo.category_l1_name = 
                    ac_Product_Category_L1.GetDataMaster((int)id, 0).
                    SearchData.category_l1_name;

                webInfo.category_l2_name = "";

                webInfo.products = ac_Product.SearchMaster(new q_Product()
                {
                    s_product_category_l1 = id,
                    s_is_open = true,
                    sidx = "sort"
                }, 0).SearchData;
            }

            if (id != null && ids != null)
            {
                a_Product_Category_L1 ac_Product_Category_L1 = new a_Product_Category_L1() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
                webInfo.category_l1_name = ac_Product_Category_L1.GetDataMaster((int)id, 0).SearchData.category_l1_name;

                a_Product_Category_L2 ac_Product_Category_L2 = new a_Product_Category_L2() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
                webInfo.category_l2_name = ac_Product_Category_L2.GetDataMaster((int)ids, 0).SearchData.category_l2_name;

                webInfo.products = ac_Product.SearchMaster(new q_Product()
                {
                    s_product_category_l2 = ids,
                    s_product_category_l1 = id,
                    s_is_open = true,
                    sidx = "sort",
                }, 0).SearchData;

            }
            return View(webInfo);
        }

        public ActionResult Product(int id)
        {
            a_Product ac_Product = new a_Product() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
            webInfo.product = ac_Product.GetDataMaster(id, 0).SearchData;

            a_Product_Category_L1 ac_Product_Category_L1 = new a_Product_Category_L1() { Connection = this.getSQLConnection(), logPlamInfo = this.plamInfo };
            webInfo.category_l1_name = ac_Product_Category_L1.GetDataMaster(webInfo.product.product_category_l1_id, 0).SearchData.category_l1_name;
            ViewBag.Product_Category_Now = webInfo.product.product_category_l1_id;

            return View(webInfo);
        }

    }

    public class ProdLite
    {
        public int id { get; set; }
        public int amt { get; set; }
        public Decimal original_price { get; set; }
        public Decimal member_price { get; set; }
        public Decimal special_price { get; set; }
        public String product_name { get; set; }
        public String imgsrc { get; set; }

    }
}
