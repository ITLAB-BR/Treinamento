using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace ITLab.Treinamento.Api.Controllers
{
    public class PermissionController : SecurityController
    {
        public JsonResult Get()
        {
            //using (var context = new AppDbContext())
            //{
            //    var result = context.Permissions
            //                        .Select(c => new
            //                        {
            //                            id = c.Id,
            //                            name = c.Name
            //                            //,description = c.Description
            //                        }).ToArray();

            //    return Json(result, JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
        //}
        }

        public JsonResult GetByUserName(string username)
        {
            using (var context = new AppDbContext())
            {
                var result = context.Users.Where(u => u.UserName == username)
                                    .SelectMany(u => u.Roles.Select(r => r.RoleId))
                                    .ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
