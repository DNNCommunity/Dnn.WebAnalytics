using DotNetNuke.Services.Exceptions;
using System;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Modules;

namespace Dnn.WebAnalytics
{
    partial class View : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                if (IsEditable)
                {
                    Actions.Add(GetNextActionID(), "Admin", ModuleActionType.AddContent, "", "", EditUrl("Admin"), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                }
                return Actions;
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
