using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Diagnostics.Contracts;
using System.Web.Script.Serialization;
using BABusiness;
using System.Threading;

namespace Breederapp
{
    /// <summary>
    /// Summary description for buitems
    /// </summary>
    public class buitems : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = -1;
            System.Net.ServicePointManager.Expect100Continue = false;

            string name = context.Request["name"];
            string buid = context.Request["buid"];
            string returnstring = string.Empty;

            if (name != null && name.Length >= 3 && buid != null)
            {
                ArrayList customers = new ArrayList();
                DataTable table = BUOrderManagement.GetAllBUItems(buid, name);

                if (table != null)
                {
                    ArrayList temp = new ArrayList();
                    string[] array = table.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).ToArray();

                    if (array != null)
                    {
                        foreach (string cont in array)
                        {
                            if (temp.Contains(cont)) continue;

                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict["text"] = cont;
                            dict["value"] = cont;
                            customers.Add(dict);
                        }
                        temp.AddRange(array);
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                returnstring = serializer.Serialize(customers);
            }

            context.Response.Write(returnstring);
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}