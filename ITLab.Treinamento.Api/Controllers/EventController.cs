using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class EventController : BaseController
    {
        private static class ResponseMessages
        {
            public const string event_not_found = "alerts-app:error.event_not_found";
            public const string event_deleted_successfully = "alerts-app:success.event_deleted_successfully";
        }

        [HttpGet]
        public JsonResult Get(EventFilterViewModel filter)
        {
            using (var context = new AppDbContext())
            {
                var query = context.Events.AsQueryable();

                if (filter.id.HasValue)
                    query = query.Where(p => p.Id == filter.id);
                else if (filter.since.HasValue)
                    query = query.Where(x => x.Date >= filter.since);
                else if (filter.until.HasValue)
                    query = query.Where(x => x.Date <= filter.until);

                var agendaEvent = query.Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Date,
                    x.Start,
                    x.End,
                    x.Color,
                    x.AllDay
                }).ToArray();

                return Json(agendaEvent, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Post(EventViewModel createEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var descr = string.IsNullOrEmpty(createEvent.Description) ? "" : createEvent.Description.Trim();
                var agendaEvent = new Event
                {
                    Description = descr,
                    Date = createEvent.Date,
                    Start = createEvent.Start.Value,
                    End = createEvent.End.HasValue ? createEvent.End.Value : new TimeSpan(),
                    Color = createEvent.Color,
                    AllDay = createEvent.AllDay
                };
                context.Events.Add(agendaEvent);
                context.SaveChanges();

                return Json(agendaEvent, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult Put(EventViewModel editEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var context = new AppDbContext())
            {
                var agendaEvent = context.Events.SingleOrDefault(c => c.Id == editEvent.Id);

                if (agendaEvent == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.event_not_found }, JsonRequestBehavior.AllowGet);
                }

                var descr = string.IsNullOrEmpty(editEvent.Description) ? "" : editEvent.Description.Trim();
                agendaEvent.Description = descr;
                agendaEvent.Date = editEvent.Date;
                agendaEvent.Start = editEvent.Start.Value;
                agendaEvent.End = editEvent.End.HasValue ? editEvent.End.Value : new TimeSpan();
                agendaEvent.Color = editEvent.Color;
                agendaEvent.AllDay = editEvent.AllDay;

                context.SaveChanges();


                return Json(agendaEvent, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult Delete(byte id)
        {
            using (var context = new AppDbContext())
            {
                var agendaEvent = context.Events.Find(id);
                if (agendaEvent == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { message = ResponseMessages.event_not_found }, JsonRequestBehavior.AllowGet);
                }

                context.Events.Remove(agendaEvent);
                context.SaveChanges();

                return Json(ResponseMessages.event_deleted_successfully, JsonRequestBehavior.AllowGet);
            }
        }
    }
}