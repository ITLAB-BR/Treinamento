using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess
{
    public partial class AppDbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}