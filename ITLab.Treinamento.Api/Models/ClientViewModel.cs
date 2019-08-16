using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;

namespace ITLab.Treinamento.Api.Models
{
    public class ClientViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "alerts:error.name_is_required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "alerts:error.email_is_required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_email")]
        [EmailAddress(ErrorMessage = "alerts:error.invalid_email")]
        public string Email { get; set; }

        [Range(minimum: 0000000000001, maximum: 9999999999999, ErrorMessage = "alerts-app:error.invalid_cnpj")]
        public Int64? CNPJ { get; set; }

        [Range(minimum: 00000000001, maximum: 99999999999, ErrorMessage = "alerts-app:error.invalid_cpf")]
        public Int64? CPF { get; set; }

        public DateTime? BirthDate { get; set; }
        public Int64? Telephone { get; set; }
        public short? Type { get; set; }
        public bool Active { get; set; }
    }
    public class ClientFilterViewModel : _DatatableDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Int64? CNPJ { get; set; }
        public Int64? CPF { get; set; }
        public Int64? Telephone { get; set; }
        public ClientTypeEnum? Type { get; set; }
        public bool? Active { get; set; }

        internal IQueryable<Client> Apply(AppDbContext context)
        {
            var query = context.Clients.AsQueryable();

            if (Active.HasValue)
                query = query.Where(c => c.Active == Active.Value);
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name));
            if (Telephone.HasValue)
                query = query.Where(c => c.Telephone.Value == Telephone.Value);
            if (Type == ClientTypeEnum.Person)
                query = query.Where(c => c.CPF.HasValue);
            else if (Type == ClientTypeEnum.Company)
                query = query.Where(c => c.CNPJ.HasValue);
            if (CPF.HasValue)
                query = query.Where(c => c.CPF.Value == CPF.Value);
            if (CNPJ.HasValue)
                query = query.Where(c => c.CNPJ.Value == CNPJ.Value);
            if (!string.IsNullOrEmpty(Email))
                query = query.Where(c => c.Email.Contains(Email));

            query = query.OrderByPropertyName(orderByColumn, orderByAsc);

            return query;
        }
    }
    public enum ClientTypeEnum
    {
        Person = 1,
        Company = 2
    }
}