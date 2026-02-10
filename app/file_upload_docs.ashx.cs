using System;
using System.Collections;
using System.IO;
using System.Web;

namespace Breederapp
{
    /// <summary>
    /// Summary description for file_upload_docs
    /// </summary>
    public class file_upload_docs : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                HttpPostedFile postedFile = context.Request.Files["file"];
                if (postedFile == null || string.IsNullOrEmpty(postedFile.FileName))
                {
                    context.Response.Write("invalid");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                    return;
                }

                string extension = Path.GetExtension(postedFile.FileName);
                if (!string.IsNullOrEmpty(extension)) extension = extension.ToLower();

                ArrayList extensionArray = new ArrayList(12);
                extensionArray.Add(".jpg");
                extensionArray.Add(".gif");
                extensionArray.Add(".png");
                extensionArray.Add(".jpeg");
                extensionArray.Add(".zip");
                extensionArray.Add(".pdf");
                extensionArray.Add(".txt");
                extensionArray.Add(".doc");
                extensionArray.Add(".docx");
                extensionArray.Add(".xls");
                extensionArray.Add(".xlsx");
                extensionArray.Add(".mp4");

                if (extensionArray.Contains(extension) == false)
                {
                    context.Response.Write("extension error ");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                    return;
                }

                string fileName = postedFile.FileName.ToLower();
                fileName = fileName.Replace(extension, "");
                fileName = fileName.Replace("-", "_");
                fileName = System.Text.RegularExpressions.Regex.Replace(fileName, "[^a-z0-9_]+", "");
                fileName = fileName.Replace(" ", "_");
                if (fileName.Length > 60) fileName = fileName.Substring(0, 60);
                fileName = DateTime.Now.Ticks + "_" + fileName + extension;

                //string targetPath = Path.Combine(context.Server.MapPath("docs"), fileName);
                //postedFile.SaveAs(targetPath);

                try
                {
                    PageBase.SaveFile(postedFile, fileName);
                }
                catch
                {
                    context.Response.Write("error");
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                }

                context.Response.Write(fileName);
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch
            {
                context.Response.Write("error");
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
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