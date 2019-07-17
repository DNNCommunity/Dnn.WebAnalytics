using System;
using System.Web;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using System.Linq;
using DotNetNuke.Services.Exceptions;

namespace Dnn.WebAnalytics
{
    public class VisitorTracker : IHttpModule
    {
        private System.Text.RegularExpressions.Regex UserAgentFilter = new System.Text.RegularExpressions.Regex(VisitController.UserAgentFilter, System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        VisitController visitController = new VisitController();
        VisitorController visitorController = new VisitorController();
        DataContext dc = new DataContext();

        public string ModuleName
        {
            get { return "VisitorTracker"; }
        }

        public void Init(HttpApplication application)
        {
            application.EndRequest += this.OnEndRequest;
        }

        public void OnEndRequest(object s, EventArgs e)
        {
            try
            {
                HttpContext Context = ((HttpApplication)s).Context;
                HttpRequest Request = Context.Request;
                HttpResponse Response = Context.Response;

                HttpCookie cookie_visitor = null;
                HttpCookie cookie_session = null;
                HttpCookie cookie_request = null;

                int visitor_id = 0;
                Nullable<int> user_id = null;
                Guid session_id = Guid.Empty;
                Guid request_id = Guid.Empty;
                Guid last_request_id = Guid.Empty;

                PortalSettings _portalSettings = (PortalSettings)Context.Items["PortalSettings"];

                // get/set cookie if visitor tracking is enabled
                cookie_visitor = Request.Cookies["DNNVISITOR"];
                if (cookie_visitor != null)
                {
                    visitor_id = Convert.ToInt32(cookie_visitor.Value);
                }

                // update/create visitor 
                var visitor = dc.Community_Visitors.Where(i => i.id == visitor_id).SingleOrDefault();
                if (visitor == null)
                { // create Visitor record 
                    visitor = new Community_Visitor()
                    {
                        created_on_date = DateTime.Now
                    };
                    dc.Community_Visitors.InsertOnSubmit(visitor);
                }

                // get User if authenticated
                if (Request.IsAuthenticated)
                {
                    UserInfo user = UserController.Instance.GetCurrentUserInfo();
                    if (user != null)
                    {
                        user_id = user.UserID;
                    }
                }

                // update the user_id if not set yet
                if (!visitor.user_id.HasValue && user_id.GetValueOrDefault() > 0)
                {
                    visitor.user_id = user_id;
                }
                dc.SubmitChanges();

                // only process requests for content pages
                if (_portalSettings != null && Request.Url.LocalPath.ToLower().EndsWith("default.aspx"))
                {
                    // filter web crawlers and other bots
                    if (String.IsNullOrEmpty(Request.UserAgent) == false && UserAgentFilter.Match(Request.UserAgent).Success == false)
                    {
                        // get last request cookie value
                        cookie_request = Request.Cookies["DNNREQUEST"];
                        if (cookie_request != null)
                        {
                            last_request_id = new Guid(cookie_request.Value);
                        }

                        // create new request cookie
                        request_id = Guid.NewGuid();
                        cookie_request = new HttpCookie("DNNREQUEST");
                        cookie_request.Value = request_id.ToString();
                        Response.Cookies.Add(cookie_request);

                        // get last session cookie value
                        cookie_session = Request.Cookies["DNNSESSION"];
                        if (cookie_session != null)
                        {
                            session_id = new Guid(cookie_session.Value);
                        }
                        else
                        {
                            // create a new session id
                            session_id = Guid.NewGuid();
                            cookie_session = new HttpCookie("DNNSESSION");
                            cookie_session.Value = session_id.ToString();
                            cookie_session.Expires = DateTime.Now.AddMinutes(30);
                            Response.Cookies.Add(cookie_session);
                        }

                        // campaign 
                        string campaign = string.Empty;
                        if (Request.QueryString["campaign"] != null)
                        {
                            campaign = Request.QueryString["campaign"];
                        }



                        // create Visitor cookie
                        cookie_visitor = new HttpCookie("DNNVISITOR");
                        cookie_visitor.Value = visitor.id.ToString();
                        cookie_visitor.Expires = DateTime.MaxValue;
                        Response.Cookies.Add(cookie_visitor);

                        string domain = Request.Url.Host + Request.ApplicationPath;
                        if (domain.EndsWith("/"))
                        {
                            domain = domain.Substring(0, domain.Length - 1);
                        }

                        // get referrer URL
                        string url_referrer = string.Empty;
                        if (Request.UrlReferrer != null)
                        {
                            url_referrer = Request.UrlReferrer.ToString();
                        }

                        string domain_referrer = string.Empty;
                        if (!string.IsNullOrEmpty(url_referrer))
                        {
                            Uri Uri = new Uri(url_referrer);
                            domain_referrer = Uri.Host;
                        }

                        // get browser language
                        string language = string.Empty;
                        if (Request.UserLanguages != null)
                        {
                            if (Request.UserLanguages.Length != 0)
                            {
                                language = Request.UserLanguages[0].ToLowerInvariant().Trim();
                            }
                        }


                        // ip address
                        string ip = Request.UserHostAddress;

                        // url
                        string url = Request.RawUrl;

                        //user agenet
                        string user_agent = Request.UserAgent;

                        // create visit object
                        VisitDTO visitDTO = new VisitDTO()
                        {
                            date = DateTime.Now,
                            visitor_id = visitor.id,
                            tab_id = _portalSettings.ActiveTab.TabID,
                            ip = ip,
                            country = "",
                            region = "",
                            city = "",
                            latitude = "",
                            longitude = "",
                            language = language,
                            domain = domain,
                            url = url,
                            user_agent = user_agent,
                            device_type = "Desktop",
                            device = "",
                            platform = "",
                            browser = "",
                            referrer_domain = domain_referrer,
                            referrer_url = url_referrer,
                            server = "",
                            activity = "click",
                            campaign = campaign,
                            session_id = session_id,
                            request_id = request_id,
                            last_request_id = last_request_id
                        };

                        visitDTO = visitController.ProcessVisit(visitDTO);

                        Community_Visit visit = visitController.ConvertDtoToItem(null, visitDTO);

                        dc.Community_Visits.InsertOnSubmit(visit);
                        dc.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        public void Dispose()
        {
        }
    }
}