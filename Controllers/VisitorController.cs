using DotNetNuke.Instrumentation;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dnn.WebAnalytics
{
    [SupportedModules("Dnn.WebAnalytics")]
    [ValidateAntiForgeryToken]
    public class VisitorController : DnnApiController
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(VisitorController));
        private VisitorInfoRepo visitorRepo = new VisitorInfoRepo();

        [NonAction]
        public VisitorDTO ConvertItemToDto(VisitorInfo item)
        {
            VisitorDTO dto = new VisitorDTO
            {
                id = item.id,
                portal_id = item.portal_id,
                user_id = item.user_id,
                created_on_date = item.created_on_date,

                user_username = item.User.Username,
                user_displayname = item.User.DisplayName
            };

            return dto;
        }
        [NonAction]
        public VisitorInfo ConvertDtoToItem(VisitorInfo item, VisitorDTO dto)
        {
            if (item == null)
            {
                item = new VisitorInfo();
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

                var visitors = visitorRepo.GetItemsAll();
                foreach (VisitorInfo visitor in visitors)
                {
                    VisitorDTO visitorDTO = ConvertItemToDto(visitor);
                    dtos.Add(visitorDTO);
                }
                return Request.CreateResponse(HttpStatusCode.OK, dtos);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
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
                VisitorInfo item = visitorRepo.GetItemById(id);

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, ConvertItemToDto(item));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
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
                Logger.Error(ex.Message, ex);
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
                Logger.Error(ex.Message, ex);
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
                VisitorInfo item = visitorRepo.GetItemById(id);

                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                visitorRepo.DeleteItem(item);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                Exceptions.LogException(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [NonAction]
        public VisitorDTO SaveVisitor(VisitorDTO dto)
        {
            VisitorInfo visitor = visitorRepo.GetItemById(dto.id);

            if (visitor == null)
            {
                visitor = ConvertDtoToItem(null, dto);
                visitor.created_on_date = DateTime.Now;

                visitor = visitorRepo.CreateItem(visitor);
            }

            return ConvertItemToDto(visitor);
        }             
    }
}