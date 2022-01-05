using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System.Collections.Generic;
using System;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _userProfileRepository;


        public PostController(IPostRepository postRepository, IPostTagRepository postTagRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IUserProfileRepository userProfileRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _userProfileRepository = userProfileRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult CommentDetails(int id)
        {
            var vm = new CommentViewModel();
            vm.PostId = id;
            vm.Comments = _commentRepository.GetAllCommentsByPostId(id);
            return View(vm);
        }
        //get
        public IActionResult AddComment(int id)
        {
            var userProfile = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email));
            Comment comment = new Comment()
            {
                PostId = id,
                UserProfileId = userProfile.Id
            };
            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //post
        public IActionResult AddComment(IFormCollection collection, Comment comment)
        {
            try
            {
                _commentRepository.Add(comment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

            public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

            public IActionResult IndexMyPosts()
        {
            int userId = GetCurrentUserProfileId();

            var posts = _postRepository.GetPostsByUserId(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var vm = new PostDetailsViewModel()
            {
                Post = _postRepository.GetPublishedPostById(id),
            };
            
            if (vm.Post == null)
            {
                int userId = GetCurrentUserProfileId();
                vm.Post = _postRepository.GetUserPostById(id, userId);
                if (vm.Post == null)
                {
                    return NotFound();
                }
            }
            vm.IsSubscribed = _postRepository.GetIsSubscribed(id, vm.Post.UserProfileId);

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAllCategories();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAllCategories();
                return View(vm);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {

                _postRepository.Update(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public IActionResult ManageTags(int id)
        {
            List<PostTag> postTags = _postTagRepository.GetPostTagsByPostId(id);
            return View(postTags);
        }

        public IActionResult AddPostTag(int id)
        {
            var vm = new AddPostTagViewModel();
            vm.PostId = id;
            vm.Tags = _tagRepository.GetTagsAvailableForPost(id);
            vm.PostTag = new PostTag()
            {
                PostId = id
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPostTag(int id, AddPostTagViewModel vm)
        {
            try
            {
                _tagRepository.AddPostTag(vm.PostTag);
                return RedirectToAction(nameof(ManageTags), new { id = vm.PostTag.PostId });
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(ManageTags), new { id = vm.PostTag.PostId });
            }
        }

        public IActionResult Subscribe(int id)
        {
            int currentUserId = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
            _userProfileRepository.Subscribe(currentUserId, id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unsubscribe(int id)
        {
            int currentUserId = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
            _userProfileRepository.Unsubscribe(currentUserId, id);
            return RedirectToAction(nameof(Index));
        }

    }
}
