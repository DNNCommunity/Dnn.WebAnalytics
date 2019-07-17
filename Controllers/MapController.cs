using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MaxMind.GeoIP2;

namespace Dnn.WebAnalytics
{
    //[SupportedModules("Dnn.WebAnalytics")]
    //[ValidateAntiForgeryToken]
    public class MapController : DnnApiController
    {
        DataContext dc = new DataContext();

        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [AllowAnonymous]
        public HttpResponseMessage Get(int minutes)
        {
            try
            {
                List<MapDTO> dtos = new List<MapDTO>();

                List<Community_Visit> recent_visits = dc.Community_Visits
                    .Where(i =>
                        i.date >= DateTime.Now.AddMinutes(-minutes) &&
                        i.latitude != "" && i.longitude != ""
                    )
                    .ToList();

                var recent_unique_visits = recent_visits
                        .GroupBy(i => i.ip)
                        .Select(i => new
                        {
                            id = i.Max(o => o.id)
                        })
                        .ToList();

                var ids = recent_unique_visits.Select(i => i.id).ToList();

                foreach (long id in ids)
                {
                    var visit = dc.Community_Visits.Where(i => i.id == id).SingleOrDefault();
                    if (visit != null)
                    {
                        MapDTO mapDTO = new MapDTO()
                        {
                            user_id = visit.Community_Visitor.user_id,
                            latitude = visit.latitude,
                            longitude = visit.longitude
                        };
                        dtos.Add(mapDTO);

                    }
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
        public HttpResponseMessage Get()
        {
            try
            {
                MapDTO dto = new MapDTO()
                {
                    latitude = "37.09024",
                    longitude = "-95.712891"
                };

                string ip_address = this.Request.GetIPAddress();

                //ip_address = "104.185.202.20"; // for testing on localhost, should resolve to Atlanta, GA...

                using (var objGeoIP2DB = new DatabaseReader(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\GeoIP2-City.mmdb")))
                {
                    try
                    {
                        var objGeoIP2 = objGeoIP2DB.City(ip_address);
                        if (objGeoIP2 != null)
                        {
                            dto.latitude = objGeoIP2.Location.Latitude.ToString();
                            dto.longitude = objGeoIP2.Location.Longitude.ToString();
                        }
                    }
                    catch
                    {
                        // IP address cannot be resolved
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, dto);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}