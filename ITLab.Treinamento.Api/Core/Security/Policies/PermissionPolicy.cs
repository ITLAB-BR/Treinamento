using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using FluentSecurity;
using FluentSecurity.Policy;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Security.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;

namespace ITLab.Treinamento.Api.Core.Security.Policies
{
    public class PermissionPolicy : ISecurityPolicy
    {
        private List<Permissions> Permissions = new List<Permissions>();
       
        public PermissionPolicy(Permissions permission)
        {
            Permissions.Add(permission);
        }

        public void Add(Permissions permission)
        {
            Permissions.Add(permission);
        }

        public PolicyResult Enforce(ISecurityContext context)
        {
            bool hasPermission;
            using (var dbContext = new AppDbContext())
            {
                hasPermission = dbContext.Users.HasPermission(Permissions);
            }

            return hasPermission ? PolicyResult.CreateSuccessResult(this) 
                                 : PolicyResult.CreateFailureResult(this, (Permissions).ToString());
        }
    }
}