using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ITLab.Treinamento.Api.Models
{
    public class CountryViewModel
    {
        public byte Id { get; set; }

        [Required(ErrorMessage = "alerts:error.name_is_required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "alerts:error.invalid_name")]
        public string Name { get; set; }
        public bool Active { get; set; }
    }

}