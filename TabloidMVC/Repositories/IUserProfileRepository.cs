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
        UserProfile GetById(int id);
        void Delete(int id);
        void Deactivate(UserProfile user);
        void Register(UserProfile userProfile);
        void Details(int id);
        void CreateUserProfile(UserProfile userProfile);
        void Update(UserProfile profile);
        UserProfile GetUserProfileById(int id);
    }
}