using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;

namespace Dnn.WebAnalytics
{
    public class ModuleBase : PortalModuleBase
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);

            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("https://use.fontawesome.com/releases/v5.7.2/css/all.css"), 1);

            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/plugins/angular-toastr/angular-toastr.min.css"), 1);

            ClientResourceManager.RegisterStyleSheet(this.Page, ResolveUrl("https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.8.0/Chart.min.css"), 2);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.8.0/Chart.bundle.min.js"), 5);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/plugins/angular-chart/angular-chart.min.js"), 6);

            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("http://cdnjs.cloudflare.com/ajax/libs/lodash.js/4.0.1/lodash.js"), 1);
                    
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular.min.js"), 2);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-messages.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-animate.min.js"), 3);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-sanitize.min.js"), 4);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-cookies.min.js"), 4);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://ajax.googleapis.com/ajax/libs/angularjs/1.7.8/angular-route.min.js"), 4);

            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/plugins/angular-toastr/angular-toastr.tpls.min.js"), 5);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/plugins/ui.bootstrap/ui-bootstrap-tpls-2.5.0.min.js"), 6);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/plugins/datetime-picker/datetime-picker.min.js"), 7);
            
            // app
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/app.js"), 7);

            // services            
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/services/visit.js"), 15);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/services/visitor.js"), 15); ;
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/services/map.js"), 15);

            // directives
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/directives/view.js"), 15);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/directives/map.js"), 15);

            // controllers
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/controllers/view.js"), 15);
            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("/DesktopModules/Dnn.WebAnalytics/app/controllers/map.js"), 15);

            ClientResourceManager.RegisterScript(this.Page, ResolveUrl("https://maps.googleapis.com/maps/api/js?key=AIzaSyBo1a4oXxfBSaUM4yEMvpKWARqyMsA1vD0"), 20);
        }
    }
}