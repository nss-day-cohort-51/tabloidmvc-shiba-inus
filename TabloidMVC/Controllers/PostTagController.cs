using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class PostTagController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostTagRepository _postTagRepository;
        //private readonly ICategoryRepository _categoryRepository;
        //private readonly ITagRepository _tagRepository;

        public PostTagController(IPostRepository postRepository, IPostTagRepository postTagRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
            //_categoryRepository = categoryRepository;
            //_tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            PostTag postTag = _postTagRepository.GetById(id);

            _postTagRepository.Delete(postTag.Id);
            
            return RedirectToAction("ManageTags", "Post", new { id = postTag.PostId });
        }
    }
}
