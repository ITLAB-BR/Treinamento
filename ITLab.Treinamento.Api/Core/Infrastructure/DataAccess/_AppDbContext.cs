using System.Data.Entity;
using System.Diagnostics;
using ITLab.Treinamento.Api.Core.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Data.Entity.Validation;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity;
using ITLab.Treinamento.Api.Core.Entities;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess
{
    public partial class AppDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>, IAppDbContext
    {
        private string username;

        public IDbSet<Group> Groups { get; set; }
        public IDbSet<AccessLog> AccessLog { get; set; }
        public IDbSet<GeneralSettings> GeneralSettings { get; set; }
        public IDbSet<APIClients> APIClients { get; set; }
        public IDbSet<APIClientRefreshToken> APIClientRefreshToken { get; set; }


        public AppDbContext()
            : base("DefaultConnection")
        {
            Database.Log = (l) => Debug.WriteLine(l);

            Database.SetInitializer<AppDbContext>(null);

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public AppDbContext WithUsername(string username)
        {
            this.username = username;
            return this;
        }

        /// <summary>
        /// Método responsável em obter as configurações e conventions e adicioná-los automaticamente 
        /// ao contexto
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assemblyTypes = typeof(AppDbContext).Assembly.GetTypes();

            var configurationTypes = assemblyTypes.Where(t => t.IsAbstract == false &&
                                                              t.BaseType != null &&
                                                              t.BaseType.IsGenericType &&
                                                              (t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                                                              t.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)))
                                                   .ToArray();

            foreach (var configurationType in configurationTypes)
            {
                dynamic configurationTypeInstance = Activator.CreateInstance(configurationType);
                modelBuilder.Configurations.Add(configurationTypeInstance);
            }

            var conventionTypes = assemblyTypes.Where(t => t.IsAbstract == false &&
                                                           t.BaseType != null &&
                                                           t.BaseType == typeof(Convention))
                                               .ToArray();

            foreach (var conventionType in conventionTypes)
            {
                dynamic configurationTypeInstance = Activator.CreateInstance(conventionType);
                modelBuilder.Conventions.Add(configurationTypeInstance);
            }

        }

        public override int SaveChanges()
        {
            try
            {
                CheckAudit();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e.TransformToThrow();
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                CheckAudit();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException e)
            {
                throw e.TransformToThrow();
            }
        }

        /// <summary>
        /// Método responsável em preencher automaticamente os campos de auditoria das entidades
        /// </summary>
        private void CheckAudit()
        {
            foreach (var itemChanged in ChangeTracker.Entries().Where(t => t.Entity is IAuditable))
            {
                if (!(itemChanged.State == EntityState.Added || itemChanged.State == EntityState.Modified))
                    continue;

                var item = itemChanged.Entity as IAuditable;

                if (itemChanged.State == EntityState.Added)
                {
                    if (item.CreationDate == default(DateTime))
                        item.CreationDate = DateTime.Now;

                    if (String.IsNullOrEmpty(item.CreationUser))
                        item.CreationUser = GetCurrentUser();
                }
                else
                {
                    item.ChangeDate = DateTime.Now;
                    item.ChangeUser = GetCurrentUser();
                }
            }
        }

        /// <summary>
        /// Retorna o usuário informado via ConfigureUsername() 
        /// ou o usuário logado no HttpContext.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUser()
        {
            if (String.IsNullOrWhiteSpace(username) && HttpContext.Current != null)
                return HttpContext.Current.User.Identity.GetUserName();

            return username;
        }

        public static int GetCurrentUserId()
        {
            if (HttpContext.Current == null) return 0;
            return Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}