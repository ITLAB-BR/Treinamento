using System.Data.Entity.ModelConfiguration.Conventions;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Conventions
{
    /// <summary>
    /// Convention que define todo campo string como varchar (e não nvarchar)
    /// </summary>
    public class UnicodeConvention : Convention
    {
        public UnicodeConvention()
        {
            Properties<string>().Configure(t => t.IsUnicode(false));
        }
    }
}
