using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
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
            var vm = new UserProfileIndexViewModel();
            vm.UserProfiles = _userProfileRepository.GetAllUsers();
            return View(vm);
        }

        // GET: UserProfileController/Details/5
        public IActionResult Details(int id)
        {
            var userProfile = _userProfileRepository.GetById(id);
            return View(userProfile);
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
        public ActionResult EditUserType(int id)
        {
            var vm = new UserProfileEditViewModel();
            vm.UserProfile = _userProfileRepository.GetById(id);
            vm.UserTypes = _userProfileRepository.GetAllTypes();

            if (vm == null)
            {
                return StatusCode(404);
            }

            return View(vm);
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserType(int id, UserProfileEditViewModel vm)
        {
            try
            {
                UserProfile userProfile = _userProfileRepository.GetById(id);
                userProfile.UserTypeId = vm.UserTypeId;
                
                _userProfileRepository.UpdateUserType(userProfile);

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
            UserProfile userProfile = _userProfileRepository.GetById(id);
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

        public IActionResult ViewDeactivated() 
        {
            var vm = new UserProfileIndexViewModel();
            vm.UserProfiles = _userProfileRepository.GetDeactivated();

            return View(vm);
        }

        public IActionResult Reactivate(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);
            user.UserTypeId = 2;
            _userProfileRepository.Update(user);

            return RedirectToAction(nameof(Index));
        }

    }
}
