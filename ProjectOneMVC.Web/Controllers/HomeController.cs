using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Data;
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
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClassService _classService;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext context,
            IMapper mapper,
            ClassService classService
            )
        {
            _logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._context = context;
            this._mapper = mapper;
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


        // GET: ClassesController
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> ClassList()
        {
            var dbClasses = await Task.Run(() => _classService.GetAll());
            var classesToRetrun = _mapper.Map<List<ClassViewModel>>(dbClasses);
            return View(classesToRetrun);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Enroll()
        {
            var classes = await _context.Classes.ToListAsync();
            var classesToReturn = _mapper.Map<List<EnrollViewModel>>(classes);
            return View(classesToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Enroll(List<EnrollViewModel> inputModel)
        {
            List<Class> dbClasses = new List<Class>();
            foreach (var item in inputModel)
            {
                if (item.Selected)
                {
                    var classToRegister = await _context.Classes.FirstOrDefaultAsync(c => c.Id == item.Id);
                    dbClasses.Add(classToRegister);
                }

            }
            var classList = new List<Class>();
            //classList.Add(classToRegister);
            // _mapper.Map <List<EnrollViewModel>>(classList)

            return View(_mapper.Map<List<EnrollViewModel>>(dbClasses));
        }


        // GET: ClassesController
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> StudentClasses()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userClasses = _classService.GetAllForUser(user.Id);

            var classesToRetrun = _mapper.Map<List<ClassViewModel>>(userClasses);
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
