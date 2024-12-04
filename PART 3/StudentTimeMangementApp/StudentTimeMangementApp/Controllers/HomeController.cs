using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentTimeMangementApp.Areas.Identity.Data;

namespace StudentTimeMangementApp.Controllers
{
    // The HomeController class handles requests related to the home page and requires authorization for access.

    [Authorize]
    public class HomeController : Controller
    {
        // Logger for logging information within the HomeController.
        private readonly ILogger<HomeController> _logger;

        // UserManager for managing user-related operations.
        private readonly UserManager<RegistrationLoginUser> _userManager;

        // Constructor for HomeController, injecting dependencies - logger and UserManager.
        public HomeController(ILogger<HomeController> logger, UserManager<RegistrationLoginUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        // Action method for the default home page (Index). Requires authorization for access.
        public IActionResult Index()
        {
            // Retrieve and set the UserID in the ViewData based on the current user.
            ViewData["UserID"] = _userManager.GetUserId(this.User);

            // Return the Index view.
            return View();
        }
    }

}
