using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EntityFramework.Extensions;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Controllers
{
    public class NotificationController : BaseController
    {
        [HttpGet]
        public JsonResult Get(int? totalRecords)
        {
            using (var context = new AppDbContext())
            {
                var query = context.Users.UserLogged()
                                          .SelectMany(c => c.Notifications)
                                          .Where(n => n.Active)
                                          .Select(n => new
                                          {
                                              id = n.Notification.Id,
                                              message = n.Notification.Message,
                                              date = n.Notification.CreationDate,
                                              readIn = n.ReadIn
                                          })
                                          .OrderByDescending(n => n.date)
                                          .AsQueryable();

                if (totalRecords.HasValue)
                    query = query.Take(totalRecords.Value);

                var result = query.ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ConfirmRead()
        {
            using (var context = new AppDbContext())
            {
                var userId = AppDbContext.GetCurrentUserId();
                context.Set<NotificationUser>().Where(n => n.UserId == userId)
                                               .Update(u => new NotificationUser { ReadIn = DateTime.Now });

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPut]
        public JsonResult MarkAsRead(int[] notifications)
        {
            using (var context = new AppDbContext())
            {
                var userId = AppDbContext.GetCurrentUserId();
                context.Set<NotificationUser>().Where(n => n.UserId == userId && notifications.Contains(n.NotificationId))
                                               .Update(u => new NotificationUser { ReadIn = DateTime.Now });

                context.SaveChanges();

                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpDelete]
        public JsonResult Remove(int[] notifications)
        {
            using (var context = new AppDbContext())
            {
                var userId = AppDbContext.GetCurrentUserId();
                context.Set<NotificationUser>().Where(n => n.UserId == userId && notifications.Contains(n.NotificationId))
                                               .Update(u => new NotificationUser { Active = false });

                context.SaveChanges();
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}