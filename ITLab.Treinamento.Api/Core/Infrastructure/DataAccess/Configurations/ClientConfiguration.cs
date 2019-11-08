using ITLab.Treinamento.Api.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            ToTable(nameof(Client));

            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("ClientId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(30)
                        .HasColumnAnnotation(
                            IndexAnnotation.AnnotationName,
                            new IndexAnnotation(
                                new IndexAttribute("IX_Client_Name") { IsUnique = true }));

            Property(t => t.Email).HasMaxLength(50);
            Property(t => t.CNPJ);
            Property(t => t.CPF);
            Property(t => t.Telephone);
            Property(t => t.BirthDate).HasColumnType("date");
            Property(t => t.Active).IsRequired();
        }
    }
}