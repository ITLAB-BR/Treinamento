using ITLab.Treinamento.Api.Core.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public abstract class SecurityController : BaseController
    {
        [HttpGet]
        protected JsonResult HasPermission(int permission)
        {
            var result = BaseController.HasPermission((Permissions)permission);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}