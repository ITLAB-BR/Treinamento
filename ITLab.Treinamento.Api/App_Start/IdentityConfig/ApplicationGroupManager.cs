using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    public class ApplicationGroupManager
    {
        private readonly ApplicationGroupStore _groupStore;
        private AppDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public IQueryable<Group> Groups { get { return _groupStore.Groups; } }

        public ApplicationGroupManager()
        {
            _db = HttpContext.Current
                .GetOwinContext().Get<AppDbContext>()
                .WithUsername(HttpContext.Current.User.Identity.GetUserName());
            _userManager = HttpContext.Current
                .GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.Current
                .GetOwinContext().Get<ApplicationRoleManager>();
            _groupStore = new ApplicationGroupStore(_db);
        }

        public ApplicationGroupManager(ApplicationGroupStore applicationGroupStore)
        {
            _groupStore = applicationGroupStore;
            _db = applicationGroupStore.Context;
        }
        public ApplicationGroupManager(ApplicationGroupStore applicationGroupStore, ApplicationRoleManager applicationRoleManager, ApplicationUserManager applicationUserManager)
        {
            _groupStore = applicationGroupStore;
            _db = applicationGroupStore.Context;
            _roleManager = applicationRoleManager;
            _userManager = applicationUserManager;
        }
        public async Task<IdentityResult> CreateGroupAsync(Group group)
        {
            await _groupStore.CreateAsync(group);
            return IdentityResult.Success;
        }

        public IdentityResult CreateGroup(Group group)
        {
            _groupStore.Create(group);
            return IdentityResult.Success;
        }
        public async Task<Group> FindByIdAsync(int id)
        {
            return await _groupStore.FindByIdAsync(id);
        }
        public Group FindById(int id)
        {
            return _groupStore.FindById(id);
        }
        public IdentityResult SetGroupRoles(int groupId, params int[] roleIds)
        {
            // Exclui todas as roles associadas ao grupo:
            var thisGroup = this.FindById(groupId);
            thisGroup.Roles.Clear();
            _db.SaveChanges();

            // Adiciona as novas roles:
            var newRoles = _roleManager.Roles.Where(r => roleIds.Any(n => n == r.Id));
            foreach (var role in newRoles)
            {
                //thisGroup.Roles.Add(new  ApplicationGroupRole { ApplicationGroupId = groupId, ApplicationRoleId = role.Id });
                thisGroup.Roles.Add(role);
            }
            _db.SaveChanges();

            // Reseta as roles de todos os usuários afetados:
            foreach (var groupUser in thisGroup.Users)
            {
                this.RefreshUserGroupRoles(groupUser.Id);
            }
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> SetGroupRolesAsync(int groupId, params int[] roleIds)
        {
            // Exclui todas as roles associadas ao grupo:
            var thisGroup = await _db.Groups.Include(g => g.Roles).Include(g => g.Users).SingleOrDefaultAsync(g => g.Id == groupId);
            thisGroup.Roles.Clear();
            await _db.SaveChangesAsync();

            // Adiciona as novas roles:
            var newRoles = _roleManager.Roles.Where(r => roleIds.Any(n => n == r.Id));
            foreach (var role in newRoles)
            {
                //thisGroup.Roles.Add(new  ApplicationGroupRole { ApplicationGroupId = groupId, ApplicationRoleId = role.Id });
                thisGroup.Roles.Add(role);
            }
            await _db.SaveChangesAsync();

            // Reseta as roles de todos os usuários afetados:
            foreach (var groupUser in thisGroup.Users)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.Id);
            }
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> SetUserGroupsAsync(int userId, params int[] groupIds)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Clear current group membership:
            var currentGroups = await this.GetUserGroupsAsync(userId);
            foreach (var group in currentGroups)
            {
                group.Users
                    .Remove(group.Users.FirstOrDefault(gr => gr.Id == userId));
            }
            await _db.SaveChangesAsync();

            // Add the user to the new groups:
            foreach (int groupId in groupIds)
            {
                var newGroup = await this.FindByIdAsync(groupId);
                newGroup.Users.Add(user);
            }
            await _db.SaveChangesAsync();

            await this.RefreshUserGroupRolesAsync(userId);
            return IdentityResult.Success;
        }
        public IdentityResult SetUserGroups(int userId, params int[] groupIds)
        {
            var user = _userManager.FindById(userId);

            // Clear current group membership:
            var currentGroups = this.GetUserGroups(userId);
            foreach (var group in currentGroups)
                group.Users.Remove(group.Users.FirstOrDefault(gr => gr.Id == userId));

            _db.SaveChanges();

            // Add the user to the new groups:
            foreach (int groupId in groupIds)
            {
                var newGroup = this.FindById(groupId);
                newGroup.Users.Add(user);
            }
            _db.SaveChanges();

            this.RefreshUserGroupRoles(userId);
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> SetGroupUsersAsync(int groupId, params int[] userIds)
        {
            // Exclui todos os usuários associados ao grupo:
            var thisGroup = await _db.Groups.Include(u => u.Users).FirstOrDefaultAsync(g => g.Id == groupId);
            var usersToRefreshRole = thisGroup.Users.ToList();
            thisGroup.Users.Clear();
            await _db.SaveChangesAsync();

            // Adiciona os novos usários:
            var newUsers = _userManager.Users.Where(r => userIds.Any(n => n == r.Id));
            foreach (var user in newUsers)
            {
                thisGroup.Users.Add(user);
                usersToRefreshRole.Add(user);
            }
            await _db.SaveChangesAsync();

            // Reseta as roles de todos os usuários afetados:
            foreach (var user in usersToRefreshRole)
                await this.RefreshUserGroupRolesAsync(user.Id);

            return IdentityResult.Success;
        }
        public IdentityResult SetGroupUsers(int groupId, params int[] userIds)
        {
            // Exclui todos os usuários associados ao grupo:
            var thisGroup = this.FindById(groupId);
            var usersToRefreshRole = thisGroup.Users.ToList();
            thisGroup.Users.Clear();
            _db.SaveChanges();

            // Adiciona os novos usários:
            var newUsers = _userManager.Users.Where(r => userIds.Any(n => n == r.Id));
            foreach (var user in newUsers)
            {
                thisGroup.Users.Add(user);
                usersToRefreshRole.Add(user);
            }
            _db.SaveChangesAsync();

            // Reseta as roles de todos os usuários afetados:
            foreach (var user in usersToRefreshRole)
                this.RefreshUserGroupRoles(user.Id);

            return IdentityResult.Success;
        }
        public IdentityResult RefreshUserGroupRoles(int userId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(User));
            }
            // Remove usuários das roles:
            var oldUserRoles = _userManager.GetRoles(userId);
            if (oldUserRoles.Count > 0)
                _userManager.RemoveFromRoles(userId, oldUserRoles.ToArray());

            // Find teh roles this user is entitled to from group membership:
            var newRolesIds = this.GetUserGroupRoles(userId).SelectMany(gr => gr.Roles.Select(r => r.Id)).ToList();

            // Get the damn role names:
            var allRoles = _roleManager.Roles.ToList();
            //var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.Id == r.Id));
            var addTheseRoles = _roleManager.Roles.Where(r => newRolesIds.Contains(r.Id));
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            //throw new ArgumentNullException(addTheseRoles.Count().ToString());

            // Add the user to the proper roles
            _userManager.AddToRoles(userId, roleNames);

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> RefreshUserGroupRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(User));
            }
            // Remove usuários das roles:
            var oldUserRoles = await _userManager.GetRolesAsync(userId);
            if (oldUserRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(userId, oldUserRoles.ToArray());

            // Find the roles this user is entitled to from group membership:
            var newRolesIds = (await this.GetUserGroupRolesAsync(userId)).SelectMany(gr => gr.Roles.Select(r => r.Id)).ToList();

            // Get the damn role names:
            var allRoles = await _roleManager.Roles.ToListAsync();
            //var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.Id == r.Id));
            var addTheseRoles = await _roleManager.Roles.Where(r => newRolesIds.Contains(r.Id)).ToArrayAsync();
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            // Add the user to the proper roles
            await _userManager.AddToRolesAsync(userId, roleNames);

            return IdentityResult.Success;
        }
        public IEnumerable<Group> GetUserGroupRoles(int userId)
        {
            var userGroups = this.GetUserGroups(userId);
            var userGroupRoles = new List<Group>();
            userGroupRoles.AddRange(userGroups);

            return userGroupRoles;
        }
        public async Task<IEnumerable<Group>> GetUserGroupRolesAsync(int userId)
        {
            var userGroups = await this.GetUserGroupsAsync(userId);
            var userGroupRoles = new List<Group>();
            userGroupRoles.AddRange(userGroups);
            
            return userGroupRoles;
        }
        public async Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            var result = new List<Group>();
            var userGroups = this.Groups
                             .Include(i => i.Roles)
                             .Include(u=> u.Users)
                             .Where(g => g.Users.Any(u => u.Id == userId))
                             .Select(g => g).ToListAsync();

            return await userGroups;
        }
        public IEnumerable<Group> GetUserGroups(int userId)
        {
            var result = new List<Group>();
            var userGroups = (from g in this.Groups
                              where g.Users.Any(u => u.Id == userId)
                              select g).ToList();
            return userGroups;
        }
        public async Task<IdentityResult> UpdateGroupAsync(Group group)
        {
            await _groupStore.UpdateAsync(group);
            foreach (var groupUser in group.Users)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.Id);
            }
            return IdentityResult.Success;
        }
        public IdentityResult UpdateGroup(Group group)
        {
            _groupStore.Update(group);
            foreach (var groupUser in group.Users)
            {
                this.RefreshUserGroupRoles(groupUser.Id);
            }
            return IdentityResult.Success;
        }
    }
}