using BABusiness;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace PetsSoftware
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BreederMail.PageURL = System.Configuration.ConfigurationManager.AppSettings["configurl"];
            BusinessBase.FixedSaltKey = System.Configuration.ConfigurationManager.AppSettings["fixedsaltkey"];
            BusinessBase.FixedDocumentHashKey = System.Configuration.ConfigurationManager.AppSettings["fixeddocumenthashkey"];
            BusinessBase.ApplicationBasePath = System.Configuration.ConfigurationManager.AppSettings["basepath"];
        }
    }
}