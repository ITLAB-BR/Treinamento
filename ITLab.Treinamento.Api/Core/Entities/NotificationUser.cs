using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    public class NotificationUser
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }

        public bool Active { get; set; }

        public virtual User User { get; set; }
        public virtual Notification Notification { get; set; }

        public DateTime? ReadIn { get; set; }
    }
}