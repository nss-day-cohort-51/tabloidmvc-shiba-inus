using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;


namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;

        }

        // GET: UserProfileController
        public ActionResult Index()
        {
            var userProfiles = _userProfileRepository.GetAllUsers();
            return View(userProfiles);
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.CreateUserProfile(userProfile);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(userProfile);
            }
        }

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return StatusCode(404);
            }

            return View(userProfile);
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Update(userProfile);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile == null)
            {
                return StatusCode(404);
            }
            return View(userProfile);
        }

        // POST: UserProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _userProfileRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Deactivate(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id, UserProfile user)
        {
            try
            {
                user.UserTypeId = 3;
                user.Id = id;
                _userProfileRepository.UpdateUserType(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
