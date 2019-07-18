using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using FiftyOne.Foundation.Mobile.Detection;
using MaxMind.GeoIP2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dnn.WebAnalytics
{
    [SupportedModules("Dnn.WebAnalytics")]
    [ValidateAntiForgeryToken]
    public class VisitController : DnnApiController
    {
        DataContext dc = new DataContext();
        public static string UserAgentFilter = "bot|crawl|spider|sbider|ask|slurp|larbin|search|indexer|archiver|nutch|capture|scanalert";

        [NonAction]
        public VisitDTO ConvertItemToDto(Community_Visit item)
        {
            VisitDTO dto = new VisitDTO();

            dto.id = item.id;
            dto.date = item.date;
            dto.visitor_id = item.visitor_id;
            dto.tab_id = item.tab_id;            
            dto.country = item.country;
            dto.region = item.region;
            dto.city = item.city;
            dto.latitude = item.latitude;
            dto.longitude = item.longitude;
            dto.language = item.language;
            dto.domain = item.domain;
            dto.url = item.url;
            dto.user_agent = item.user_agent;
            dto.device_type = item.device_type;
            dto.device = item.device;
            dto.platform = item.platform;
            dto.browser = item.browser;
            dto.referrer_domain = item.referrer_domain;
            dto.referrer_url = item.referrer_url;
            dto.server = item.server;
            dto.campaign = item.campaign;
            dto.session_id = item.session_id;
            dto.request_id = item.request_id;
            dto.last_request_id = item.last_request_id;

            return dto;
        }
        [NonAction]
        public Community_Visit ConvertDtoToItem(Community_Visit item, VisitDTO dto)
        {
            if (item == null)
            {
                item = new Community_Visit();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.date = dto.date;
            item.visitor_id = dto.visitor_id;
            item.tab_id = dto.tab_id;            
            item.country = dto.country;
            item.region = dto.region;
            item.city = dto.city;
            item.latitude = dto.latitude;
            item.longitude = dto.longitude;
            item.language = dto.language;
            item.domain = dto.domain;
            item.url = dto.url;
            item.user_agent = dto.user_agent;
            item.device_type = dto.device_type;
            item.device = dto.device;
            item.platform = dto.platform;
            item.browser = dto.browser;
            item.referrer_domain = dto.referrer_domain;
            item.referrer_url = dto.referrer_url;
            item.server = dto.server;
            item.campaign = dto.campaign;
            item.session_id = dto.session_id;
            item.request_id = dto.request_id;
            item.last_request_id = dto.last_request_id;

            return item;
        }

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                List<VisitDTO> dtos = new List<VisitDTO>();

                var query = dc.Community_Visits.AsQueryable();

                foreach (Community_Visit item in query)
                {
                    VisitDTO dto = ConvertItemToDto(item);
                    dtos.Add(dto);
                }

                return Request.CreateResponse(HttpStatusCode.OK, dtos);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Community_Visit item = dc.Community_Visits.Where(i => i.id == id).SingleOrDefault();

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, ConvertItemToDto(item));
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage Get(int portal_id, Nullable<DateTime> period_start, Nullable<DateTime> period_end)
        {
            try
            {
                var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

                if (period_start.HasValue)
                {
                    query = query.Where(i => i.date.Date >= period_start.GetValueOrDefault().Date);
                }

                if (period_end.HasValue)
                {
                    query = query.Where(i => i.date.Date <= period_end.GetValueOrDefault().Date);
                }

                var list = query.ToList();

                DashboardDTO dashboardDTO = new DashboardDTO()
                {
                    view_count = list.Count(),
                    visit_count = list.Select(i => i.session_id).Distinct().Count(),
                    visitor_count = list.Select(i => i.visitor_id).Distinct().Count(),
                    user_count = list.Where(i => i.Community_Visitor.user_id.HasValue).Select(i => i.visitor_id).Distinct().Count(),

                    views = GetViews(portal_id, period_start, period_end),
                    visits = GetVisits(portal_id, period_start, period_end),
                    visitors = GetVisitors(portal_id, period_start, period_end),
                    users = GetUsers(portal_id, period_start, period_end),

                };
                return Request.CreateResponse(HttpStatusCode.OK, dashboardDTO);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage Get(string field, int portal_id, Nullable<DateTime> period_start, Nullable<DateTime> period_end, int rows)
        {
            try
            {
                List<ReportDTO> results = new List<ReportDTO>();

                var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

                if (period_start.HasValue)
                {
                    query = query.Where(i => i.date.Date >= period_start.GetValueOrDefault().Date);
                }

                if (period_end.HasValue)
                {
                    query = query.Where(i => i.date.Date <= period_end.GetValueOrDefault().Date);
                }

                var list = query.ToList();

                IEnumerable<IGrouping<string, Community_Visit>> grouped = null;

                switch (field)
                {
                    case "date":
                        grouped = list.GroupBy(i => i.date.Date.ToShortDateString());
                        break;

                    case "weekday":
                        grouped = list.GroupBy(i => i.date.DayOfWeek.ToString());
                        break;

                    case "month":
                        grouped = list.GroupBy(i => i.date.ToString("MMMM"));
                        break;

                    case "hour":
                        grouped = list.GroupBy(i => i.date.Hour.ToString());
                        break;

                    case "user":
                        grouped = list.Where(i => i.Community_Visitor.user_id.HasValue).GroupBy(i => i.Community_Visitor.User.DisplayName);
                        break;

                    case "page":
                        grouped = list.GroupBy(i => i.Tab.TabPath);
                        break;

                    case "referrer_domain":
                        grouped = list.GroupBy(i => i.referrer_domain);
                        break;

                    case "device_type":
                        grouped = list.GroupBy(i => i.device_type);
                        break;

                    case "domain":
                        grouped = list.GroupBy(i => i.domain);
                        break;

                    case "url":
                        grouped = list.GroupBy(i => i.url);
                        break;
                                           
                    case "country":
                        grouped = list.GroupBy(i => i.country);
                        break;

                    case "region":
                        grouped = list.GroupBy(i => i.region);
                        break;

                    case "city":
                        grouped = list.GroupBy(i => i.city);
                        break;

                    case "language":
                        grouped = list.GroupBy(i => i.language);
                        break;

                    case "user_agent":
                        grouped = list.GroupBy(i => i.user_agent);
                        break;

                    case "device":
                        grouped = list.GroupBy(i => i.device);
                        break;

                    case "platform":
                        grouped = list.GroupBy(i => i.platform);
                        break;

                    case "browser":
                        grouped = list.GroupBy(i => i.browser);
                        break;

                    case "campaign":
                        grouped = list.GroupBy(i => i.campaign);
                        break;

                    case "server":
                        grouped = list.GroupBy(i => i.server);
                        break;

                    default:
                        break;
                }

                results = grouped
                            .Select(i => new ReportDTO()
                            {
                                field = i.Key,
                                count = i.Count()
                            })
                            .OrderByDescending(i => i.count).
                            Take(rows)
                            .ToList();

                int total = results.Sum(i => i.count);

                foreach (ReportDTO result in results)
                {
                    if (string.IsNullOrEmpty(result.field))
                    {
                        result.field = "Unknown";
                    }
                    result.total = total;
                    result.percent = (int)(((float)result.count / (float)total) * 100);
                }

                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage Post(VisitDTO dto)
        {
            try
            {
                dto = SaveVisit(dto);

                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage Put(VisitDTO dto)
        {
            try
            {
                dto = SaveVisit(dto);

                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Community_Visit item = dc.Community_Visits.Where(i => i.id == id).SingleOrDefault();

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dc.Community_Visits.DeleteOnSubmit(item);
                dc.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [NonAction]
        public VisitDTO SaveVisit(VisitDTO dto)
        {
            Community_Visit visit = dc.Community_Visits.Where(i => i.id == dto.id).SingleOrDefault();

            if (visit == null)
            {
                visit = ConvertDtoToItem(null, dto);

                dc.Community_Visits.InsertOnSubmit(visit);
            }

            visit = ConvertDtoToItem(visit, dto);

            dc.SubmitChanges();

            return ConvertItemToDto(visit);
        }

        //[NonAction]
        //public void WriteVisits()
        //{
        //    List<VisitDTO> visit_dtos = new List<VisitDTO>();

        //    // dictionary to store visitors to update
        //    Dictionary<int, Nullable<int>> dicVisitors = new Dictionary<int, Nullable<int>>();

        //    // get all visitor objects from Cache
        //    dynamic CacheItems = HttpRuntime.Cache.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).Where(key => key.StartsWith("DNNVISITOR")).ToArray();

        //    // iterate through visit items
        //    foreach (string Key in CacheItems)
        //    {
        //        // get visitor object
        //        VisitDTO visit_dto = (VisitDTO)HttpRuntime.Cache.Get(Key);

        //        // populate visit fields
        //        visit_dto = ProcessVisit(visit_dto);

        //        Visit visit = ConvertDtoToItem(null, visit_dto);
        //        dc.Visits.InsertOnSubmit(visit);
        //        dc.SubmitChanges();

        //        //// save visitor 
        //        //if (!dicVisitors.ContainsKey(visit_dto.visitor_id))
        //        //{
        //        //    dicVisitors.Add(visit_dto.visitor_id, visit_dto..user_id);
        //        //}
        //        //else
        //        //{
        //        //    dicVisitors[visit_dto.visitor_id] = visit_dto.user_id;
        //        //}

        //        HttpRuntime.Cache.Remove(Key);                
        //    }

        //    //// iterate through all visitors that need to be updated
        //    //foreach (KeyValuePair<int, Nullable<int>> kvp in dicVisitors)
        //    //{
        //    //    visitorController.UpdateVisitor(kvp.Key, kvp.Value);
        //    //}
        //}

        [NonAction]
        public VisitDTO ProcessVisit(VisitDTO visit)
        {
            // get server
            visit.server = Dns.GetHostName();

            //visit.ip = "104.185.202.20"; // for testing on localhost, should resolve to Atlanta, GA...

            // get geo info based on IP 
            if (!string.IsNullOrEmpty(visit.ip) && visit.ip != "127.0.0.1")
            {
                using (var objGeoIP2DB = new DatabaseReader(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\GeoIP2-City.mmdb")))
                {
                    try
                    {
                        var objGeoIP2 = objGeoIP2DB.City(visit.ip);

                        if (objGeoIP2.Country.Name != null && objGeoIP2.Country.Name != "N/A")
                        {
                            visit.country = objGeoIP2.Country.Name;
                        }
                        if (objGeoIP2.MostSpecificSubdivision.Name != null)
                        {
                            visit.region = objGeoIP2.MostSpecificSubdivision.Name;
                        }
                        if (objGeoIP2.City.Name != null)
                        {
                            visit.city = objGeoIP2.City.Name;
                        }
                        visit.latitude = objGeoIP2.Location.Latitude.ToString();
                        visit.longitude = objGeoIP2.Location.Longitude.ToString();
                    }
                    catch
                    {
                        // IP address cannot be resolved
                    }
                }
            }

            //get user agent properties using the 51Degrees database
            if (!string.IsNullOrEmpty(visit.user_agent))
            {
                var device = WebProvider.ActiveProvider.Match(visit.user_agent);
                if (device != null)
                {
                    if (device["IsMobile"] != null && device["IsMobile"].ToString() == "True")
                    {
                        visit.device_type = "Mobile";
                    }

                    if (device["HardwareVendor"] != null && device["HardwareVendor"].ToString() != "Unknown")
                    {
                        visit.device += device["HardwareVendor"].ToString(); // only available in Premium Data
                    }
                    if (device["HardwareModel"] != null && device["HardwareModel"].ToString() != "Unknown")
                    {
                        visit.device += device["HardwareModel"]; // only available in Premium Data
                    }
                    if (visit.device == "")
                    {
                        visit.device = "Unavailable";
                    }

                    if (device["PlatformVendor"] != null && device["PlatformVendor"].ToString() != "Unknown")
                    {
                        visit.platform += device["PlatformVendor"] + " "; // only available in Premium Data
                    }
                    if (device["PlatformName"] != null && device["PlatformName"].ToString() != "Unknown")
                    {
                        visit.platform += device["PlatformName"] + " ";
                    }
                    if (device["PlatformVersion"] != null && device["PlatformVersion"].ToString() != "Unknown")
                    {
                        visit.platform += device["PlatformVersion"];
                    }
                    if (visit.platform == "")
                    {
                        visit.platform = "Unavailable";
                    }

                    if (device["BrowserVendor"] != null && device["BrowserVendor"].ToString() != "Unknown")
                    {
                        visit.browser += device["BrowserVendor"] + " "; // only available in Premium Data
                    }
                    if (device["BrowserName"] != null && device["BrowserName"].ToString() != "Unknown")
                    {
                        visit.browser += device["BrowserName"] + " ";
                    }
                    if (device["BrowserVersion"] != null && device["BrowserVersion"].ToString() != "Unknown")
                    {
                        visit.browser += device["BrowserVersion"];
                    }
                    if (visit.browser == "")
                    {
                        visit.browser = "Unavailable";
                    }
                }
            }

            return visit;
        }

        [NonAction]
        public void PurgeVisits()
        {
            // delete all visit history older than 90 days
            List<Community_Visit> visits = dc.Community_Visits.Where(i => i.date.Date < DateTime.Now.AddDays(-90).Date).ToList();
            dc.Community_Visits.DeleteAllOnSubmit(visits);
            dc.SubmitChanges();
        }

        [NonAction]
        public List<DateCountDTO> GetViews(int portal_id, Nullable<DateTime> start_date, Nullable<DateTime> end_date)
        {
            var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

            if (start_date.HasValue)
            {
                query = query.Where(i => i.date.Date >= start_date.GetValueOrDefault().Date);
            }

            if (end_date.HasValue)
            {
                query = query.Where(i => i.date.Date <= end_date.GetValueOrDefault().Date);
            }

            var list = query.ToList();

            var results = list
                            .GroupBy(i => i.date.Date)
                            .Select(i => new DateCountDTO()
                            {
                                date = i.Key,
                                count = i.Count()
                            })
                            .OrderBy(i => i.date)
                            .ToList();

            return results;
        }

        [NonAction]
        public List<DateCountDTO> GetVisits(int portal_id, Nullable<DateTime> start_date, Nullable<DateTime> end_date)
        {
            var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

            if (start_date.HasValue)
            {
                query = query.Where(i => i.date.Date >= start_date.GetValueOrDefault().Date);
            }

            if (end_date.HasValue)
            {
                query = query.Where(i => i.date.Date <= end_date.GetValueOrDefault().Date);
            }

            var list = query.ToList();

            var results = list
                         .GroupBy(i => i.date.Date)
                         .Select(i => new DateCountDTO()
                         {
                             date = i.Key,
                             count = i.Select(o => o.session_id).Distinct().Count()
                         })
                         .OrderBy(i => i.date)
                         .ToList();

            return results;
        }

        [NonAction]
        public List<DateCountDTO> GetVisitors(int portal_id, Nullable<DateTime> start_date, Nullable<DateTime> end_date)
        {
            var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

            if (start_date.HasValue)
            {
                query = query.Where(i => i.date.Date >= start_date.GetValueOrDefault().Date);
            }

            if (end_date.HasValue)
            {
                query = query.Where(i => i.date.Date <= end_date.GetValueOrDefault().Date);
            }

            var list = query.ToList();

            var results = list
                         .GroupBy(i => i.date.Date)
                         .Select(i => new DateCountDTO()
                         {
                             date = i.Key,
                             count = i.Select(o => o.visitor_id).Distinct().Count()
                         })
                         .OrderBy(i => i.date)
                         .ToList();

            return results;
        }

        [NonAction]
        public List<DateCountDTO> GetUsers(int portal_id, Nullable<DateTime> start_date, Nullable<DateTime> end_date)
        {
            var query = dc.Community_Visits.Where(i => i.Tab.PortalID == portal_id);

            if (start_date.HasValue)
            {
                query = query.Where(i => i.date.Date >= start_date.GetValueOrDefault().Date);
            }

            if (end_date.HasValue)
            {
                query = query.Where(i => i.date.Date <= end_date.GetValueOrDefault().Date);
            }

            var list = query.ToList();

            var results = list
               .GroupBy(i => i.date.Date)
               .Select(i => new DateCountDTO()
               {
                   date = i.Key,
                   count = i.Select(o => o.Community_Visitor.user_id).Distinct().Count()
               })
               .OrderBy(i => i.date)
               .ToList();

            return results;
        }

    }
}