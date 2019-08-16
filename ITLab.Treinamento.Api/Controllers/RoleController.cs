using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class RoleController : SecurityController
    {
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public JsonResult List()
        {
            var result = RoleManager.Roles.Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
            })
            .OrderBy(o=>o.Name)
            .ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}