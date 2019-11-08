using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Core.Security;
using ITLab.Treinamento.Api.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class APIClientController : BaseController
    {
        private static class ResponseMessages
        {
            public const string other_apiclient_already_exists = "alerts-app:error.other_apiclient_already_exists";
            public const string apiclient_not_found = "alerts-app:error.apiclient_not_found";
            public const string apiclient_deleted_successfully = "alerts-app:success.apiclient_deleted_successfully";
        }

        [HttpGet]
        public JsonResult Get(string id)
        {
            using (var context = new AppDbContext())
            {
                var query = context.APIClients.AsQueryable();

                if (!string.IsNullOrEmpty(id))
                    query = query.Where(p => p.Id == id);

                var apiClient = query.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    p.AllowedOrigin,
                    p.RefreshTokenLifeTimeInMinutes,
                    p.Active
                }).ToArray();

                return Json(apiClient, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Post(APIClientCreateViewModel createApiClient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            createApiClient.Id = createApiClient.Id.Trim().Replace(" ", "");

            using (var context = new AppDbContext())
            {
                var apiClientExists = context.APIClients.Any(p => p.Id == createApiClient.Id);
                if (apiClientExists)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { message = ResponseMessages.other_apiclient_already_exists }, JsonRequestBehavior.AllowGet);
                }

                var apiClient = new APIClients
                {
                    Id = createApiClient.Id,
                    Name = createApiClient.Name.Trim(),
                    Secret = SecurityHelper.GetHash(createApiClient.Secret.Trim()),
                    Type = createApiClient.Type,
                    AllowedOrigin = createApiClient.AllowedOrigin.Trim(),
                    RefreshTokenLifeTimeInMinutes = createApiClient.RefreshTokenLifeTimeInMinutes,
                    Active = true,
                };
                context.APIClients.Add(apiClient);
                context.SaveChanges();

                apiClient.Secret = null;

                return Json(apiClient, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult Put(APIClientEditViewModel editApiClient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            editApiClient.Id = editApiClient.Id.Trim().Replace(" ", "");

            using (var context = new AppDbContext())
            {
                var apiClient = context.APIClients.SingleOrDefault(c => c.Id == editApiClient.Id);

                if (apiClient == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.apiclient_not_found }, JsonRequestBehavior.AllowGet);
                }

                apiClient.Name = editApiClient.Name.Trim();
                apiClient.Type = editApiClient.Type;
                apiClient.AllowedOrigin = editApiClient.AllowedOrigin.Trim();
                apiClient.RefreshTokenLifeTimeInMinutes = editApiClient.RefreshTokenLifeTimeInMinutes;
                apiClient.Active = editApiClient.Active;

                context.SaveChanges();

                apiClient.Secret = null;

                return Json(apiClient, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPut]
        [Route("api/APIClient/Secret/Put")]
        public JsonResult PutSecret(APIClientEditSecretViewModel editApiClientSecret)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            editApiClientSecret.Id = editApiClientSecret.Id.Trim().Replace(" ", "");

            using (var context = new AppDbContext())
            {
                var apiClient = context.APIClients.SingleOrDefault(c => c.Id == editApiClientSecret.Id);

                if (apiClient == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.apiclient_not_found }, JsonRequestBehavior.AllowGet);
                }

                apiClient.Secret = SecurityHelper.GetHash(editApiClientSecret.Secret.Trim());
                context.SaveChanges();

                apiClient.Secret = null;

                return Json(apiClient, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { message = ResponseMessages.apiclient_not_found }, JsonRequestBehavior.AllowGet);
            }

            id = id.Trim().Replace(" ", "");

            using (var context = new AppDbContext())
            {
                var apiClient = context.APIClients.Find(id);
                if (apiClient == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.apiclient_not_found }, JsonRequestBehavior.AllowGet);
                }

                apiClient.Active = false;
                context.SaveChanges();

                return Json(ResponseMessages.apiclient_deleted_successfully, JsonRequestBehavior.AllowGet);
            }
        }
    }
}