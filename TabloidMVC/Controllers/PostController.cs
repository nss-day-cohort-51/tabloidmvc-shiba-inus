﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System.Collections.Generic;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
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
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
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
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public IActionResult ManageTags(int id)
        {
            List<Tag> tags = _tagRepository.GetTagsByPostId(id);
            return View(tags);
        }

        public IActionResult AddPostTag(int id)
        {
            var vm = new AddPostTagViewModel();
            vm.PostId = id;
            vm.Tags = _tagRepository.GetTagsAvailableForPost(id);
            vm.PostTag = new PostTag() {
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
                return RedirectToAction(nameof(ManageTags), vm.PostId);
            }
        }

    }
}
