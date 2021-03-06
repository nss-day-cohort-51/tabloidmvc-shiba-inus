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
        void UpdateUserType(UserProfile user);
        void Register(UserProfile userProfile);
        void CreateUserProfile(UserProfile userProfile);
        void Subscribe(int currentUserId, int id);
        void Unsubscribe(int currentUserId, int id);
        List<UserProfile> GetDeactivated();
        void Update(UserProfile userProfile);
        List<UserType> GetAllTypes();
    }
}