using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using WebApplication8.ViewModels.Account;

namespace WebApplication8.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager)

        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
       
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname= registerVM.Surname,
                UserName = registerVM.UserName,

            };
            IdentityResult result=await _userManager.CreateAsync(appUser,registerVM.Password);
            if(result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, "Bu adda category movcuddur");
                }
                return View();

            }
            await _signInManager.SignInAsync(appUser,false);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if(appUser == null)
            {
                appUser=await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if(appUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Bele hesab yoxdur");
                    return View();
                }
            }
            await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, false, true);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout() { return View(); }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {   await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
