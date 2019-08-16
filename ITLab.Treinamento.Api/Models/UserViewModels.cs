using System.Collections.Generic;

namespace ITLab.Treinamento.Api.Models
{
    public class NewUserViewModels
    {
        public int AuthenticationTypeId { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class UserViewModels
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public bool AccessAllDataVisibility { get; set; }
        public List<byte> Countries { get; set; }
        public List<int> Groups { get; set; }


        public UserViewModels()
        {
            Countries = new List<byte>();
            Groups = new List<int>();
        }
    }

    public class UserFilterViewModel
    {
        public bool? Active { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RemovePhoto { get; set; }
    }
}