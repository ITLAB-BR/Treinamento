using ITLab.Treinamento.Api.Core.Entities.Security;
using System.ComponentModel.DataAnnotations;

namespace ITLab.Treinamento.Api.Models
{
    public class APIClientCreateViewModel
    {
        [Required(ErrorMessage = "alerts:error.apiclientid_is_required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_id")]
        public string Id { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientname_is_required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientsecret_is_required")]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "alerts:error.invalid_secret")]
        public string Secret { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclienttype_is_required")]
        public APIClientTypes Type { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientallowedorigin_is_required")]
        public string AllowedOrigin { get; set; }
        public int RefreshTokenLifeTimeInMinutes { get; set; }
        public bool Active { get; set; }
    }

    public class APIClientEditViewModel
    {
        [Required(ErrorMessage = "alerts:error.apiclientid_is_required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_id")]
        public string Id { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientname_is_required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclienttype_is_required")]
        public APIClientTypes Type { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientallowedorigin_is_required")]
        public string AllowedOrigin { get; set; }
        public int RefreshTokenLifeTimeInMinutes { get; set; }
        public bool Active { get; set; }
    }

    public class APIClientEditSecretViewModel
    {
        [Required(ErrorMessage = "alerts:error.apiclientid_is_required")]
        public string Id { get; set; }
        [Required(ErrorMessage = "alerts:error.apiclientsecret_is_required")]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "alerts:error.invalid_secret")]
        public string Secret { get; set; }
    }
}