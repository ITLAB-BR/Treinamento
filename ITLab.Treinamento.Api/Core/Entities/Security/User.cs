using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ITLab.Treinamento.Api.Core.Configuration;
using System.Drawing;
using System.IO;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IAuditable
    {
        public User()
        {
            Countries = new List<Country>();
            Group = new List<Group>();
            PreviousUserPasswords = new List<PreviousUserPasswords>();
            Notifications = new List<NotificationUser>();
        }

        public string Name { get; set; }
        public bool Active { get; set; }
        public bool AccessAllDataVisibility { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<Group> Group { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public virtual ICollection<PreviousUserPasswords> PreviousUserPasswords { get; set; }
        public virtual ICollection<NotificationUser> Notifications { get; set; }

        public virtual UserPhoto UserPhoto { get; set; }

        public DateTime? LastPasswordChangedDate { get; set; }

        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }

        public bool IsPasswordExpired()
        {
            var daysLeftToChangePassword = this.DaysLeftToChangePassword();

            return daysLeftToChangePassword == null ? false : (daysLeftToChangePassword == 0);
        }

        public DateTime? DateThatMustChangePassword()
        {
            DateTime? result = null;

            if (this.ExpirePasswordIsSet())
            {
                var daysLeftToChangePassword = this.DaysLeftToChangePassword();

                if (daysLeftToChangePassword != null)
                    result = DateTime.Now.Date.AddDays((int)daysLeftToChangePassword);
            }

            return result;
        }

        public int? DaysLeftToChangePassword()
        {
            int? result = null;

            if (this.ExpirePasswordIsSet())
            {
                var settings = SettingHelper.Get();
                var daysSinceLastChange = (int)(DateTime.Now.Date - (DateTime)this.LastPasswordChangedOrCreatedDate()).TotalDays;
                
                var differenceDays = settings.PasswordExpiresInDays - daysSinceLastChange;
                var daysLeftToChangePassword = differenceDays > 0 ? differenceDays : 0;

                result = daysLeftToChangePassword;
            }

            return result;
        }

        public DateTime? LastPasswordChangedOrCreatedDate()
        {
            DateTime? lastPasswordChangedOrCreatedDate = null;

            if (this.AuthenticationType == AuthenticationType.DataBase)
                lastPasswordChangedOrCreatedDate = this.LastPasswordChangedDate.HasValue ? ((DateTime)this.LastPasswordChangedDate).Date : this.CreationDate.Date;

            return lastPasswordChangedOrCreatedDate;
        }

        private bool ExpirePasswordIsSet()
        {
            var settings = SettingHelper.Get();
            return (this.AuthenticationType == AuthenticationType.DataBase) && (settings.PasswordExpiresInDays != 0);
        }

        public Image GetPhoto()
        {
            var userPhotoMemoryStream = new MemoryStream(this.UserPhoto.Photo);
            var image = Image.FromStream(userPhotoMemoryStream);

            return image;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public Notification Notify(string message)
        {
            var notification = new Notification { Message = message };
            Notifications.Add(new NotificationUser { User = this, Notification = notification, Active = true });

            return notification;
        }

        public Notification NotifyFileGenerated(string fileName)
        {
            var notification = new Notification { Message = "{{commons.generated_file}} {{url_api}}files?fileName=" + fileName };
            Notifications.Add(new NotificationUser { User = this, Notification = notification, Active = true });

            return notification;
        }

        internal void ChangePhoto(byte[] photo)
        {
            if (UserPhoto == null)
                UserPhoto = new UserPhoto { User = this };

            UserPhoto.Photo = photo;
        }
    }

    public class UserLogin : IdentityUserLogin<int>
    {
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public enum AuthenticationType : byte
    {
        DataBase = 1,
        ActiveDirectory = 2
    }
}
