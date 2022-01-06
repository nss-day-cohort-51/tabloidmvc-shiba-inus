using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

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
                return RedirectToAction("Index", new {id = comment.PostId});
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
