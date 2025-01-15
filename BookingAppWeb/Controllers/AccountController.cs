using BookingApp.Application.Common.Interfaces;
using BookingApp.Application.Common.Utility;
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
                RedirectUrl = returnUrl,
            };

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register (string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                // .Wait() asteapta ca task-ul sa fie complet executat
                // .Wait() => inlocuitor pentru async & await
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).Wait();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_PropertyOwner)).Wait(); 
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl,
            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                // daca rezultatul este un succes
                if (result.Succeeded)
                {
                    // ADAUGARE/ASIGNARE ROL USER
                    // daca exista cel putin un rol selectat
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        // se adauga un singur rol -> AddToRolesAsync pt mai multe roluri
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    // daca nu exista niciun rol selectat, userul primeste automat rolul de Customer
                    else
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetails.Role_Customer);
                    }

                    // isPersistent:false - sign in the user in mod automat
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // redirectionam catre:
                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        // pagina de Home - daca nu exista redirectURL
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Url - ul Redirectionat - daca exista redirectURL
                        // LocalRedirect -> redirectionam mereu catre domeniul local
                        // * nu punem direct link-ul pentru a nu redirectiona catre site-uri malitioase
                        // * (security measure)
                        return LocalRedirect(registerVM.RedirectUrl);
                    }

                }

                // daca rezultatul nu este un succes
                // afisam eroarea
                foreach (var error in result.Errors)
                {
                    // nu adaugam KEY, se va afisa eroarea de pe UI (asp-validation-summary = "ModelOnly" class = "text-danger")
                    ModelState.AddModelError("", error.Description);
                }
            }
            // populam lista row
            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
        
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                // verificare User & Parola
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure:false);

                // daca rezultatul este un succes
                if (result.Succeeded)
                {
                    // redirectionam catre:
                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        // pagina de Home - daca nu exista redirectURL
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Url - ul Redirectionat - daca exista redirectURL
                        // LocalRedirect -> redirectionam mereu catre domeniul local
                        // * nu punem direct link-ul pentru a nu redirectiona catre site-uri malitioase
                        // * (security measure)
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
                // daca rezultatul nu este un succes
                // => nu vrem sa aratam utilizatorului eroarea
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }
    }
}
