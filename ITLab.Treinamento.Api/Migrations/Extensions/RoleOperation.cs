using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Migrations.Extensions
{
    public enum CreateOrDrop
    {
        Create,
        Drop
    }
    public class RoleOperation : MigrationOperation
    {
        public string RoleName { get; set; }
        public CreateOrDrop Operation { get; set; }
        public RoleOperation(CreateOrDrop operation, string roleName)
            : base(null)
        {
            RoleName = roleName;
            Operation = operation;
        }
        public override string ToString()
        {
            return string.Format("{0} ROLE [{1}]", Operation.ToString().ToUpper(), RoleName);
        }
        public override bool IsDestructiveChange
        {
            get { return false; }
        }
    }
}