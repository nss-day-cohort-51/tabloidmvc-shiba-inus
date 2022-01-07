using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Models;


namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        public CommentController(ICommentRepository commentRepository, IUserProfileRepository userProfileRepository)

        {
            _commentRepository = commentRepository;
            _userProfileRepository = userProfileRepository;

        }
        // GET: CommentController
        public ActionResult Index(int id)
        {
            var vm = new CommentViewModel();
            vm.PostId = id;
            vm.Comments = _commentRepository.GetAllCommentsByPostId(id);
            return View(vm);
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            var userProfile = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email));
            Comment comment = new Comment()
            {
                PostId = id,
                UserProfileId = userProfile.Id
            };
            return View(comment);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Comment comment)
        {
            try
            {
                _commentRepository.Add(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }



        // GET: CommentController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userProfile = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email));
            Comment comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Comment comment)
        {
            try
            {
                _commentRepository.Update(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }



        // GET: CommentController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userProfile = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email));
            Comment comment = _commentRepository.GetCommentById(id);


            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment, int postId)
        {
            try
            {
              
                _commentRepository.Delete(id);

                return RedirectToAction("Index", new { id = postId });
            }
            catch (Exception ex)
            { 
                return View(comment);
            }
        }
    }
}
