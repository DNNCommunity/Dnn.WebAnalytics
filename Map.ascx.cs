using DotNetNuke.Services.Exceptions;
using System;

namespace Dnn.WebAnalytics
{
    public partial class Map : ModuleBase
    {
        public string GoogleAPIKey
        {
            get {
                return "AIzaSyBo1a4oXxfBSaUM4yEMvpKWARqyMsA1vD0";
            }            
        }
        protected new void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

    }

}