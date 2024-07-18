using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace AdminDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber!,
                    UserName = user.UserName!,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                usersViewModel.Add(userViewModel);

            }

            return View(usersViewModel);


        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allroles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRoleViewModel()
            {
                UserId = user!.Id,
                UserName = user.UserName!,
                Roles = allroles.Select(R => new RoleViewModel() 
                {
                    Id = R.Id,
                    Name = R.Name!,
                    IsSelected = _userManager.IsInRoleAsync(user,R.Name!).Result
                }).ToList(),
            };


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user!);
            foreach (var role in model.Roles) 
            { 
                if(userRoles.Any(R => R == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user!, role.Name);
                
                if (!userRoles.Any(R => R == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user!, role.Name);
                
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
