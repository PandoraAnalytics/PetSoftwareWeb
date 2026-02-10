using BABusiness;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace Breederapp
{
    public class search_breed : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                string searchterm = context.Request["q"];
                if (string.IsNullOrEmpty(searchterm))
                {
                    context.Response.Write("");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                }

                NameValueCollection collection = new NameValueCollection();
                collection["start_name"] = searchterm;
                string filter = UserBA.Search(collection);
                collection = null;

                DataSet ds = UserBA.GetUser_AnimalDetails(1, filter, context.Session["userid"]);
                string[] array = (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows.Cast<DataRow>().Select(row => row["name"].ToString()).ToArray() : null;

                if (array != null && array.Length > 0)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string serialized = serializer.Serialize(array);
                    serializer = null;
                    context.Response.Write(serialized);
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                }
                else
                {
                    context.Response.Write("");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                }
            }
            catch
            {
                context.Response.Write("");
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
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