using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectOneMVC.Web.DTO;
using ProjectOneMVC.Web.Services;
using ProjectOneMVC.Web.ViewModels;

namespace ProjectOneMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ClassService _classService;
        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ClassService classService
            )
        {
            _logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._classService = classService;
        }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        #region Identity
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel inputModel, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = inputModel.Email, Email = inputModel.Email };
                var result = await _userManager.CreateAsync(user, inputModel.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = ReturnUrl;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel loginInputModel, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(loginInputModel.Email, loginInputModel.Password, loginInputModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(ReturnUrl = ReturnUrl ?? Url.Content("~/"));
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return View();
            }
        }
        #endregion Identity



        // GET: ClassesController
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ClassList()
        {
            var classViewModel = _classService.GetAll<ClassViewModel>();
            return View(classViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Enroll()
        {
            var classViewModel = _classService.GetAll<EnrollViewModel>();
            return View(classViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Enroll(List<EnrollViewModel> inputModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var results = await _classService.AssignClassFor(user.Id, inputModel);
            
            if (results.Success)
            {
                return RedirectToAction(nameof(StudentClasses), "Home");
            }

            // If we get this far, provice feedback
            ViewData["Error"] = "Class already assigned!";
            return View(_classService.GetAll<EnrollViewModel>());
        }


        // GET: ClassesController
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> StudentClasses()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var classesToRetrun = _classService.GetAllForUser<ClassViewModel>(user.Id);

            return View(classesToRetrun);
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
