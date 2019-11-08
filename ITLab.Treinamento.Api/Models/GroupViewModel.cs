using System.Collections.Generic;

namespace ITLab.Treinamento.Api.Models
{
    public class GroupViewModel
    {
        public string Name { get; set; }
    }

    public class EditGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GroupPermissionsViewModel
    {
        public int GroupId { get; set; }
        public List<int> RolesIds { get; set; }

        public GroupPermissionsViewModel()
        {
            RolesIds = new List<int>();
        }
    }

    public class GroupUsersViewModel
    {
        public int GroupId { get; set; }
        public List<int> UsersIds { get; set; }

        public GroupUsersViewModel()
        {
            UsersIds = new List<int>();
        }
    }
}