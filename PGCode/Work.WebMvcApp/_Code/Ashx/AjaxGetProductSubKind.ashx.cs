using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    public class AjaxGetProductSubKind : BaseIHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            vmJsonResult r_json_data = null;
            int id = int.Parse(context.Request.Form["master_value"]);
            try
            {
                a_Product_Category_L2 LBase = new a_Product_Category_L2() { Connection = getSQLConnection(), logPlamInfo = new ProcCore.Log.LogPlamInfo() { } };

                r_json_data = new vmJsonResult() { result = true, data = LBase.SearchMaster(new q_Product_Category_L2() { s_product_category_l1_id = id }, 0).SearchData.ToDictionary(x => x.id, x => x.category_l2_name) };
                context.Response.Write(JsonConvert.SerializeObject(r_json_data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            }
            catch (Exception ex)
            {
                r_json_data = new vmJsonResult() { result = false, message = ex.Message + ":" + ex.StackTrace };
                context.Response.Write(JsonConvert.SerializeObject(r_json_data));
            }
        }
    }
}