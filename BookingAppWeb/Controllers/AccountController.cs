using BookingApp.Application.Common.Interfaces;
using BookingApp.Domain.Entities;
using BookingAppWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingAppWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl=null)
        {
            //if returnUrl !null => return content
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                RedirectURL = returnUrl,
            };


            return View(loginVM);
        }
        public IActionResult Register ()
        {
            if(!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                // .Wait() asteapta ca task-ul sa fie complet executat
                // .Wait() => inlocuitor pentru async & await
                _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                _roleManager.CreateAsync(new IdentityRole("Customer")).Wait();
                _roleManager.CreateAsync(new IdentityRole("PropertyOwner")).Wait(); 
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(registerVM);
        }
    }
}
