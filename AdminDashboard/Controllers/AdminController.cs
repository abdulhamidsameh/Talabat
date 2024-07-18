using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Identity;

namespace AdminDashboard.Controllers
{
	public class AdminController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminController(SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager)
        {
			_signInManager = signInManager;
			_userManager = userManager;
		}
        public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDto login)
		{
			var user = await _userManager.FindByEmailAsync(login.Email);
			if (user is null)
			{
				ModelState.AddModelError("Invalid Login", "Invalid Data");
				return RedirectToAction(nameof(Login));
			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password,false);
			if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "Admin"))
			{
				ModelState.AddModelError("Invalid Login", "Not Authorized");
				return RedirectToAction(nameof(Login));
			}

			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
	}
}
