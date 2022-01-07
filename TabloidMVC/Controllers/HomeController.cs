using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostRepository _postRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, IUserProfileRepository userProfileRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Index()
        {
            if (User.FindFirstValue(ClaimTypes.Email) != null)
            {
                int currentUserId = _userProfileRepository.GetByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
                List<Post> subscribedPosts = _postRepository.GetAllSubscribedPosts(currentUserId);
                return View(subscribedPosts);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
