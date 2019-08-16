using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace ITLab.Treinamento.Api.Controllers
{
    public class CountryController : BaseController
    {
        private static class ResponseMessages
        {
            public const string other_country_already_exists = "alerts-app:error.other_country_already_exists";
            public const string country_not_found = "alerts-app:error.country_not_found";
            public const string country_deleted_successfully = "alerts-app:success.country_deleted_successfully";
        }

        [HttpGet]
        public JsonResult Get(int? id, bool onlyByUserLogged = false, bool onlyActive = false)
        {
            using (var context = new AppDbContext())
            {
                var query = onlyByUserLogged ? context.GetCountriesByUserLogged() : context.Countries.AsQueryable();

                if (onlyActive)
                    query = query.Where(c => c.Active);


                if (id.HasValue)
                    query = query.Where(p => p.Id == id);

                var country = query.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Active
                }).ToArray();

                return Json(country, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Post(CountryViewModel createCountry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var countryExists = context.Countries.Any(p => p.Name == createCountry.Name);
                if (countryExists)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { message = ResponseMessages.other_country_already_exists }, JsonRequestBehavior.AllowGet);
                }

                var country = new Country
                {
                    Name = createCountry.Name.Trim(),
                    Active = true,
                };
                context.Countries.Add(country);
                context.SaveChanges();

                return Json(country, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult Put(CountryViewModel editCountry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var country = context.Countries.SingleOrDefault(c => c.Id == editCountry.Id);

                if (country == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.country_not_found }, JsonRequestBehavior.AllowGet);
                }

                var otherCountryExists = context.Countries.Any(p => p.Name == editCountry.Name && p.Id != editCountry.Id);
                if (otherCountryExists)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return Json(new { message = ResponseMessages.other_country_already_exists }, JsonRequestBehavior.AllowGet);
                }

                country.Name = editCountry.Name.Trim();
                country.Active = editCountry.Active;
                context.SaveChanges();

                return Json(country, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult Delete(byte id)
        {
            using (var context = new AppDbContext())
            {
                var country = context.Countries.Find(id);
                if (country == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.country_not_found }, JsonRequestBehavior.AllowGet);
                }

                country.Active = false;
                context.SaveChanges();

                return Json(ResponseMessages.country_deleted_successfully, JsonRequestBehavior.AllowGet);
            }
        }
    }
}