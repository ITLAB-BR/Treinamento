using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess
{
    public interface IAppDbContext
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Role> Roles { get; set; }

        Database Database { get; }
        DbContextConfiguration Configuration { get; }

        int SaveChanges();
        void Dispose();
    }
}