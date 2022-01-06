using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserProfileEditViewModel
    {
        public int UserTypeId { get; set; }
        public List<UserType> UserTypes { get; set; }
        public UserProfile UserProfile { get; set; }

    }
}
