using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;

namespace ITLab.Treinamento.Api.Migrations.Extensions
{
    public enum Permission
    {
        Select,
        Update,
        Delete,
        Insert,
        CRUD
    }
    public enum GrantOrRevoke
    {
        Grant,
        Revoke
    }

    public class PermissionOperation : MigrationOperation
    {
        public PermissionOperation(GrantOrRevoke opreation, string schema, string table, string user, Permission permission)
          : base(null)
        {
            Schema = schema;
            Table = table;
            UserOrRole = user;
            Permission = permission;
        }

        public string Schema { get; set; }
        public string Table { get; private set; }
        public string UserOrRole { get; private set; }
        public Permission Permission { get; private set; }
        public GrantOrRevoke Operation { get; set; }

        public override string ToString()
        {
            if (Operation == GrantOrRevoke.Grant)
                return string.Format("GRANT {0} ON [{1}].[{2}] TO {3}", Permission.ToString().ToUpper(), Schema, Table, UserOrRole); 
            else if (Operation == GrantOrRevoke.Revoke)
                return string.Format("REVOKE {0} ON [{1}].[{2}] FROM {3}", Permission.ToString().ToUpper(), Schema, Table, UserOrRole);
            else
                throw new InvalidOperationException("Operation not valid!");
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}
