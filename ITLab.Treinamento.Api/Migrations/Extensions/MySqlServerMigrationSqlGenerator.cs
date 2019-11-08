using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.SqlServer;

namespace ITLab.Treinamento.Api.Migrations.Extensions
{
    public class MySqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            var operationPermission = migrationOperation as PermissionOperation;

            if (operationPermission != null)
            {
                using (var writer = Writer())
                {
                    writer.WriteLine(operationPermission.ToString());
                    Statement(writer);
                }
            }

            var operationRole = migrationOperation as RoleOperation;

            if (operationRole != null)
            {
                using (var writer = Writer())
                {
                    writer.WriteLine(operationRole.ToString());
                    Statement(writer);
                }
            }
        }
    }
}
