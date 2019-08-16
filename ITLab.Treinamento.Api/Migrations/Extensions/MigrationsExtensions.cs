using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace ITLab.Treinamento.Api.Migrations.Extensions
{
    public static class Extensions
    {
        static List<Permission> permissionList = new List<Permission>();
        private static void SetPermissionList(Permission permission)
        {
            permissionList = new List<Permission>();
            if (permission == Permission.CRUD)
            {
                permissionList.Add(Permission.Insert);
                permissionList.Add(Permission.Update);
                permissionList.Add(Permission.Delete);
                permissionList.Add(Permission.Select);
            }
            else
                permissionList.Add(permission);
        }
        public static void GrantPermission(this DbMigration migration, string schema, string table, string user, Permission permission)
        {
            SetPermissionList(permission);
            foreach (var item in permissionList)
            {
                ((IDbMigration)migration)
                  .AddOperation(new PermissionOperation(GrantOrRevoke.Grant, schema, table, user, item));
            }
        }
        public static void RevokePermission(this DbMigration migration, string schema, string table, string user, Permission permission)
        {
            SetPermissionList(permission);
            foreach (var item in permissionList)
            {
                ((IDbMigration)migration)
              .AddOperation(new PermissionOperation(GrantOrRevoke.Revoke, schema, table, user, item));
            }
        }

        public static void CreateRole(this DbMigration migration, string roleName)
        {
            ((IDbMigration)migration)
              .AddOperation(new RoleOperation(CreateOrDrop.Create, roleName));
        }

        public static void DropRole(this DbMigration migration, string roleName)
        {
            ((IDbMigration)migration)
              .AddOperation(new RoleOperation(CreateOrDrop.Drop, roleName));
        }
    }
}
