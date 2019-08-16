using ITLab.Treinamento.Api.Core.Entities;
using System.Linq;
using ITLab.Treinamento.Api.Core.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess
{
    public static class DataVisibilityQueries
    {
        /// <summary>
        /// Retorna as unidades de negócio do usuário logado
        /// </summary>
        /// <param name="context">DbSet da unidade de negócio mapeado no AppDbContext</param>
        /// <returns>Retorna as unidades de negócio relacionadas ao usuário logado</returns>
        public static IQueryable<Country> GetDataVisibilityByUserLogged(this AppDbContext context)
        {

            var query = context.Countries.Select(c => c);

            if (!SecurityIdentityHelper.CurrentUserHasAccessAllDataVisibility())
            {
                var userId = SecurityIdentityHelper.CurrentUserId();
                query = query.Where(g => g.Users.Any(u => u.Id == userId));
            }

            return query;
        }
        public static IQueryable<Country> GetCountriesByUserLogged(this AppDbContext context)
        {
            return GetDataVisibilityByUserLogged(context);
        }
    }
}