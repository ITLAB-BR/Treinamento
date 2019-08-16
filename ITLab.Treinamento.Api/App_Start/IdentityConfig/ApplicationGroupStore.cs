using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    public class ApplicationGroupStore : IDisposable
    {
        private bool _disposed;
        public AppDbContext Context { get; private set; }
        public DbSet<Group> DbEntitySet { get; private set; }
        public IQueryable<Group> EntitySet { get { return this.DbEntitySet; } }
        public IQueryable<Group> Groups { get { return this.EntitySet; } }

        public bool DisposeContext { get; set; }

        public ApplicationGroupStore(AppDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.Context = context;
            this.DbEntitySet = context.Set<Group>();
        }

        public virtual void Create(Group group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            this.DbEntitySet.Add(group);
            this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(Group group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            this.DbEntitySet.Add(group);
            await this.Context.SaveChangesAsync();
        }
        public Task<Group> FindByIdAsync(int groupId)
        {
            this.ThrowIfDisposed();
            return this.GetByIdAsync(groupId);
        }

        public Group FindById(int groupId)
        {
            this.ThrowIfDisposed();
            return this.GetById(groupId);
        }
        public virtual Task<Group> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[] { id });
        }
        public virtual Group GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }
        public virtual async Task UpdateAsync(Group group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            this.Context.Entry<Group>(group).State = EntityState.Modified;
            await this.Context.SaveChangesAsync();
        }
        public virtual void Update(Group group)
        {
            this.ThrowIfDisposed();
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            this.Context.Entry<Group>(group).State = EntityState.Modified;
            this.Context.SaveChanges();
        }
        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
            }
            this._disposed = true;
            this.Context = null;
        }
    }
}