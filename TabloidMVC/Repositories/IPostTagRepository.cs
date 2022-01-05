using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostTagRepository
    {
        PostTag GetById(int id);
        void Delete(int id);
        List<PostTag> GetPostTagsByPostId(int id);
    }
}