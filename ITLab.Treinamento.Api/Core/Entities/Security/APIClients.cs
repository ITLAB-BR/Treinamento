using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class APIClients : IAuditable
    {
        public APIClients()
        {
            APIClientRefreshTokens = new List<APIClientRefreshToken>();
        }

        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public APIClientTypes Type { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTimeInMinutes { get; set; }
        public string AllowedOrigin { get; set; }

        public virtual ICollection<APIClientRefreshToken> APIClientRefreshTokens { get; set; }

        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }

        internal void AddRefreshToken(APIClientRefreshToken refreshToken)
        {
            APIClientRefreshTokens.Add(refreshToken);
        }
    }

    public enum APIClientTypes : byte
    {
        JavaScriptNonconfidencial = 1,
        NativeConfidencial = 2
    }
}