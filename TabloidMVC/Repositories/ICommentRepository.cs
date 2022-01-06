using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        public List<Comment> GetAllCommentsByPostId(int id);
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(int id);
        Comment GetCommentById(int id);

    }
}
