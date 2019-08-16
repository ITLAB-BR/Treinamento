using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    public class Notification : IAuditable
    {
        public Notification()
        {
            Users = new List<NotificationUser>();
        }

        public int Id { get; set; }

        public virtual ICollection<NotificationUser> Users { get; set; }

        public string Message { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreationUser { get; set; }

        public void To(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                Users.Add(new NotificationUser { Notification = this, User = user, Active = true });
            }
        }
    }
}