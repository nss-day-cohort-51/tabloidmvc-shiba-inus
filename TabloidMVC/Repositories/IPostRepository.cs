using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Update(Post post);

        List<Post> GetAllPublishedPosts();
        List<Post> GetPostsByUserId(int id);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void Delete(int id);
        bool GetIsSubscribed(int id1, int id2);
        List<Post> GetAllSubscribedPosts(int id);
    }
}