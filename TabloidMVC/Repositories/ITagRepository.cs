using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        void CreateTag(Tag tag);
        void Delete(int id);
        Tag GetTagById(int id);
        void Update(Tag tag);
        List<Tag> GetTagsByPostId(int postId);
        List<Tag> GetTagsAvailableForPost(int postId);
        void AddPostTag(PostTag postTag);
    }
}
