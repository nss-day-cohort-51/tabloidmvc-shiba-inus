using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAllUsers();
        UserProfile GetByEmail(string email);
        void Details(int id);
        void CreateUserProfile(UserProfile userProfile);
        void Update(UserProfile profile);
        void Delete(int id);
        UserProfile GetUserProfileById(int id);
    }
}