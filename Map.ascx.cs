using DotNetNuke.Services.Exceptions;
using System;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace Dnn.WebAnalytics
{
    public partial class Map : ModuleBase
    {
        public string GoogleAPIKey
        {
            get
            {
                if (!string.IsNullOrEmpty((string)Settings["google_api_key"]))
                {
                    return (string)Settings["google_api_key"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected new void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://maps.googleapis.com/maps/api/js?key=" + GoogleAPIKey), 20);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected string ApiUrlBase
        {
            get
            {
                if (DotNetNuke.Application.DotNetNukeContext.Current.Application.CurrentVersion.Major < 9)
                {
                    return "/desktopmodules/Dnn.WebAnalytics/api";
                }
                return "/api/Dnn.WebAnalytics";
            }
        }

    }

}