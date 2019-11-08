using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class GroupController : SecurityController
    {
        private ApplicationGroupManager _groupManager;
        public ApplicationGroupManager GroupManager
        {
            get { return _groupManager ?? new ApplicationGroupManager(); }
            private set { _groupManager = value; }
        }
        public JsonResult List()
        {
            var result = GroupManager.Groups.Select(g => new
            {
                Id = g.Id,
                Name = g.Name,
                Roles = g.Roles.Select(r => r.Id),
                Users = g.Users.Select(u => u.Id)
            })
            .OrderBy(o => o.Name)
            .ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> CreateAsync(GroupViewModel editedGroup)
        {
            var group = new Group { Name = editedGroup.Name };
            //TODO: Verificar como tratar erros em chamdas assincronas.
            var result = await GroupManager.CreateGroupAsync(group);
            if (!result.Succeeded)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json("erro_inesperado", JsonRequestBehavior.AllowGet);
            }

            return Json(group, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UpdateAsync(EditGroupViewModel editedGroup)
        {
            var group = await GroupManager.FindByIdAsync(editedGroup.Id);
            group.Name = editedGroup.Name;
            var result = await GroupManager.UpdateGroupAsync(group);

            return null;
        }
        public async Task<JsonResult> UpdatePermissionsAsync(GroupPermissionsViewModel groupPermissions)
        {
            var group = await GroupManager.SetGroupRolesAsync(groupPermissions.GroupId, groupPermissions.RolesIds.ToArray());

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> UpdateUsersAsync(GroupUsersViewModel groupUsers)
        {
            var result = await GroupManager.SetGroupUsersAsync(groupUsers.GroupId, groupUsers.UsersIds.ToArray());

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}