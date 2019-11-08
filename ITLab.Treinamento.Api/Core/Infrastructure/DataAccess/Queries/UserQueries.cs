using System.Data.Entity;
using System.Linq;
using System.Web;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Security.Authorization;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess
{
    public static class UserQueries
    {
        /// <summary>
        /// Retorna um IQueryable de usuário a partir do usuário logado
        /// </summary>
        /// <param name="source">DbSet do usuário mapeado no identity</param>
        /// <returns></returns>
        public static IQueryable<User> UserLogged(this IDbSet<User> source)
        {
            return source.AsQueryable().UserLogged();
        }

        public static IQueryable<User> UserLogged(this IQueryable<User> source)
        {
            var id = HttpContext.Current.User.Identity.GetUserId<int>();
            return source.Where(u => u.Id == id);
        }

        public static bool HasPermission(this IDbSet<User> source, Permissions permission)
        {
            var permissions = new List<Permissions>();
            permissions.Add(permission);

            return HasPermission(source, permissions);
        }


        /// <summary>
        /// Retorna um boleano dizendo se o usuário logado possui tal permissão ou não.
        /// </summary>
        /// <param name="source">DbSet do usuário mapeado no identity</param>
        /// <param name="permissions">Será verificada se o usuário a possui esta permissão</param>
        /// <returns></returns>
        public static bool HasPermission(this IDbSet<User> source, List<Permissions> permissions)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId<int>();

            return source.Where(u => u.Id == userId)
                         .SelectMany(u => u.Roles)
                         .Select(u => u.Role)
                         .Any(r => permissions.Contains((Permissions)r.Id));
        }
    }
}