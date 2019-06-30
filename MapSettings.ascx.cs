using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System.Web.UI.WebControls;

namespace Dnn.WebAnalytics
{
    public partial class MapSettings : ModuleSettingsBase
	{

        protected TextBox txtGoogle;
        
        public override void LoadSettings()
		{
			try {
				if (!Page.IsPostBack) {
                    if (!string.IsNullOrEmpty((string)ModuleSettings["google_api_key"]))
                    {
                        txtGoogle.Text = (string)ModuleSettings["google_api_key"];
                    }                  
                }
            } catch (Exception exc) {
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		public override void UpdateSettings()
		{
			try {
                ModuleController.Instance.UpdateModuleSetting(ModuleId, "google_api_key", txtGoogle.Text);                
            }
            catch (Exception exc) {
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

	}

}

