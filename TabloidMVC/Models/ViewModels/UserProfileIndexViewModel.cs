using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserProfileIndexViewModel
    {
        public List<UserProfile> UserProfiles { get; set; }
        public bool IsAdmin { get; set; }

    }
}
