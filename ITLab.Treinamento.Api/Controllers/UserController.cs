using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using ITLab.Treinamento.Api.Core.Upload;
using System.Data.Entity;
using ITLab.Treinamento.Api.Core.Entities;

namespace ITLab.Treinamento.Api.Controllers
{
    public class UserController : SecurityController
    {
        private ApplicationGroupManager groupManager;
        public ApplicationGroupManager GroupManager
        {
            get { return groupManager ?? new ApplicationGroupManager(); }
            private set { groupManager = value; }
        }

        [HttpGet]
        public JsonResult GetList(UserFilterViewModel filter)
        {
            using (var context = new AppDbContext())
            {
                var query = context.Users.Select(c => new
                {
                    Id = c.Id,
                    Active = c.Active,
                    AuthenticationTypeId = (int)c.AuthenticationType,
                    Name = c.Name,
                    Email = c.Email,
                });

                if (filter.Active.HasValue)
                    query = query.Where(u => u.Active == filter.Active.Value);
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(u => u.Name.Contains(filter.Name));
                if (!string.IsNullOrWhiteSpace(filter.Email))
                    query = query.Where(u => u.Email.Contains(filter.Email));

                var result = query.ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetItem(int id)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Include(u => u.Group)
                                        .Include(u => u.Countries)
                                        .SingleOrDefault(u => u.Id == id);


                if (user == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new ArgumentException("User not found"), JsonRequestBehavior.AllowGet);
                }
                {
                    var userIsLockedOut = UserManager.IsLockedOut(id);
                    var userLockoutEndDate = userIsLockedOut ? (DateTimeOffset?)UserManager.GetLockoutEndDate(id) : null;

                    var result = new
                    {
                        user.Id,
                        user.Active,
                        AuthenticationType = user.AuthenticationType == AuthenticationType.ActiveDirectory ? "AD" : "Banco de Dados",
                        user.Name,
                        Login = user.UserName,
                        user.Email,
                        user.AccessAllDataVisibility,
                        IsPasswordExpired = user.IsPasswordExpired(),
                        LastPasswordChangedDate = user.LastPasswordChangedOrCreatedDate(),
                        DateThatMustChangePassword = user.DateThatMustChangePassword(),
                        UserBlockedForManyAccess = userIsLockedOut,
                        LockoutEndDateUtc = userLockoutEndDate,
                        DaysLeftToChangePassword = user.DaysLeftToChangePassword(),
                        Groups = user.Group.Select(r => r.Id),
                        Countries = user.Countries.Select(b => b.Id)
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync(NewUserViewModels request)
        {
            var user = new User
            {
                Active = true,
                AuthenticationType = (AuthenticationType)request.AuthenticationTypeId,
                Name = request.Name,
                UserName = request.Login,
                Email = request.Email,
                CreationUser = UserLoggedUserName
            };

            var result = await CreateUserByTypeAuthenticateAsync(user, request.Password);

            if (result == null || !result.Succeeded)
            {
                SetHttpContextResponseStatusCode(result);
                return Json(SanitizeResult(result), JsonRequestBehavior.AllowGet);
            }

            return Json(user.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public async Task<JsonResult> UpdateAsync(UserViewModels request)
        {
            var user = await UserManager.FindByIdAsync(request.Id);
            user.Active = request.Active;
            user.Name = request.Name;
            user.AccessAllDataVisibility = request.AccessAllDataVisibility;
            user.ChangeDate = DateTime.Now;
            user.ChangeUser = UserLoggedUserName;

            var resultSetUserGroupsAsync = await GroupManager.SetUserGroupsAsync(request.Id, request.Groups.ToArray());
            AddOrRemoveDataVisibility(request);

            var result = await UserManager.UpdateAsync(user);

            if (result == null || !result.Succeeded)
            {
                SetHttpContextResponseStatusCode(result);
                return Json(SanitizeResult(result), JsonRequestBehavior.AllowGet);
            }

            return Json(user.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmail(int id)
        {

            using (var context = new AppDbContext())
            {
                var result = context.Users
                    .Where(c => c.Id == id)
                    .Select(c => new { c.Email })
                    .SingleOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetMyAccount()
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.SingleOrDefault(c => c.UserName == UserLoggedUserName);

                var result = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Login = user.UserName,
                    Email = user.Email,
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public string GetMyAvatar()
        {
            using (var context = new AppDbContext())
            {
                var userPhoto = context.Users
                    .Include(u => u.UserPhoto)
                    .Where(c => c.Id == UserLoggedId)
                    .Select(u => u.UserPhoto.Photo)
                    .SingleOrDefault();


                if (userPhoto == null)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    return null;
                }

                var result = string.Concat("data:image/JPEG;base64,", Convert.ToBase64String(userPhoto));

                return result;
            }
        }

        [HttpPut]
        public async Task<JsonResult> UpdateMyAccountAsync(EditUserViewModel editedUser)
        {
            var user = await UserManager.FindByIdAsync(UserLoggedId);
            user.Name = editedUser.Name;
            user.ChangeDate = DateTime.Now;
            user.ChangeUser = UserLoggedUserName;

            var resultSaveBasicData = await UserManager.UpdateAsync(user);

            if (resultSaveBasicData == null || !resultSaveBasicData.Succeeded)
            {
                SetHttpContextResponseStatusCode(resultSaveBasicData);
                return Json(SanitizeResult(resultSaveBasicData), JsonRequestBehavior.AllowGet);
            }

            if (Request.Files.Count >= 1)
                SavePhoto(Request, user.Id);

            if (editedUser.RemovePhoto)
                DeletePhoto(user.Id);

            return Json(user.Id, JsonRequestBehavior.AllowGet);
        }

        private static void AddOrRemoveDataVisibility(UserViewModels editedUser)
        {
            //NOTE: Não conseguimos associar a DataVisibility ao usuário, por isso fizemos o inverso (usuário à DataVisibility)
            using (var context = new AppDbContext())
            {
                var user = context.Users.Include(d => d.Countries).FirstOrDefault(u => u.Id == editedUser.Id);

                user.Countries.Clear();

                if (!editedUser.AccessAllDataVisibility)
                {
                    var countriesToAdd = editedUser.Countries.Select(c =>
                    {
                        return context.Countries.Local.FirstOrDefault(i => i.Id == c) ?? context.Countries.Attach(new Country { Id = c });
                    }).ToArray();

                    foreach (var item in countriesToAdd)
                    {
                        user.Countries.Add(item);
                    }
                }

                context.SaveChanges();
            }
        }

        private async Task<IdentityResult> CreateUserByTypeAuthenticateAsync(User user, string password)
        {
            if (user.AuthenticationType == AuthenticationType.DataBase)
                return !SettingHelper.Get().AuthenticateDataBase ? new IdentityResult("authenticate_database_disabled") : await UserManager.CreateAsync(user, password);

            else if (user.AuthenticationType == AuthenticationType.ActiveDirectory)
                return !SettingHelper.Get().AuthenticateActiveDirectory ? new IdentityResult("authenticate_activedirectory_disabled") : await UserManager.CreateAsync(user);

            else
                throw new ArgumentOutOfRangeException("User.Type is invalid");
        }

        private void SavePhoto(HttpRequestBase request, int userId)
        {
            var userPhotoPath = Request.Files.Save().SingleOrDefault();

            using (var context = new AppDbContext())
            {
                var user = context.Users.Include(u => u.UserPhoto).SingleOrDefault(u => u.Id == userId);
                user.ChangePhoto(UploadHelper.GetFileAsImage(userPhotoPath));

                context.SaveChanges();
            }

            UploadHelper.DeleteFileFromDirectoryTemp(userPhotoPath);
        }

        private static void DeletePhoto(int userId)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Include(u => u.UserPhoto).SingleOrDefault(u => u.Id == userId);
                user.UserPhoto = null;
                context.SaveChanges();
            }
        }
    }
}