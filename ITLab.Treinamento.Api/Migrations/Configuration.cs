using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Migrations.Extensions;
using System.Data.Entity.Migrations;

namespace ITLab.Treinamento.Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            //Defina o ambiente para o qual se executará o migrations 
            Environment.EnvironmentTpe = Environment.Type.DesenvolvimentoLocal;

            //Defina o nome da role
            DataBaseConfig.DataBaseRoleName = "grp_App_ITLabTreinamento";

            //Registra nosso gerador customizado
            SetSqlGenerator("System.Data.SqlClient", new MySqlServerMigrationSqlGenerator());
        }

        protected override void Seed(AppDbContext context)
        {
            //Este método é chamado logo após aplicar o último migration
            var initializeDataBase = new Seed(context);
            initializeDataBase.InitialDataBase();
            Migrations.Seed.InsertDataTest();
        }
    }

    public static class Environment
    {
        public enum Type
        {
            DesenvolvimentoLocal = 1,
            HomologacaoITLab = 2
        }

        public static Type EnvironmentTpe { get; set; }
    }

    public static class DataBaseConfig
    {
        public static string DataBaseRoleName;
    }
}
