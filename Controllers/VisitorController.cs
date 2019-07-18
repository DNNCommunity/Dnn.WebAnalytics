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

namespace Dnn.WebAnalytics
{
    [SupportedModules("Dnn.WebAnalytics")]
    [ValidateAntiForgeryToken]
    public class VisitorController : DnnApiController
    {
        DataContext dc = new DataContext();

        [NonAction]
        public VisitorDTO ConvertItemToDto(Community_Visitor item)
        {
            VisitorDTO dto = new VisitorDTO();

            dto.id = item.id;
            dto.portal_id = item.portal_id;
            dto.user_id = item.user_id;
            dto.created_on_date = item.created_on_date;

            dto.user_username = item.User.Username;
            dto.user_displayname = item.User.DisplayName;

            return dto;
        }
        [NonAction]
        public Community_Visitor ConvertDtoToItem(Community_Visitor item, VisitorDTO dto)
        {
            if (item == null)
            {
                item = new Community_Visitor();
            }

            if (dto == null)
            {
                return item;
            }

            item.id = dto.id;
            item.portal_id = dto.portal_id;
            item.user_id = dto.user_id;
            item.created_on_date = dto.created_on_date;

            return item;
        }
        
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                List<VisitorDTO> dtos = new List<VisitorDTO>();

                var visitors = dc.Community_Visitors.ToList();
                foreach (Community_Visitor visitor in visitors)
                {
                    VisitorDTO visitorDTO = ConvertItemToDto(visitor);
                    dtos.Add(visitorDTO);
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
                Community_Visitor item = dc.Community_Visitors.Where(i => i.id == id).SingleOrDefault();

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

        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage Post(VisitorDTO dto)
        {
            try
            {
                dto = SaveVisitor(dto);

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
        public HttpResponseMessage Put(VisitorDTO dto)
        {
            try
            {
                dto = SaveVisitor(dto);

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
                Community_Visitor item = dc.Community_Visitors.Where(i => i.id == id).SingleOrDefault();

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                dc.Community_Visitors.DeleteOnSubmit(item);
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
        public VisitorDTO SaveVisitor(VisitorDTO dto)
        {
            Community_Visitor visitor = dc.Community_Visitors.Where(i => i.id == dto.id).SingleOrDefault();

            if (visitor == null)
            {
                visitor = ConvertDtoToItem(null, dto);
                visitor.created_on_date = DateTime.Now;

                dc.Community_Visitors.InsertOnSubmit(visitor);
            }

            visitor = ConvertDtoToItem(visitor, dto);

            dc.SubmitChanges();

            return ConvertItemToDto(visitor);
        }             
    }
}