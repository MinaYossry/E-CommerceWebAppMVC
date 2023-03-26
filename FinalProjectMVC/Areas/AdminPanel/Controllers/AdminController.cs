using FinalProjectMVC.Areas.AdminPanel.Models;
using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]

    public class AdminController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;


            public AdminController(UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUserRoles()
            {
                var users = await _userManager.Users.ToListAsync();
                var roles = await _roleManager.Roles.ToListAsync();

                var viewModel = new ManageUserRolesViewModel
                {
                    Users = users,
                    Roles = roles
                };

                return View(viewModel);
            }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetUserRole(string SelectedUserId, string SelectedRoleId)
        {
            var user = await _userManager.FindByIdAsync(SelectedUserId);
            var role = await _roleManager.FindByIdAsync(SelectedRoleId);

            if (user != null && role != null)
            {
                // Remove user from any existing roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                // Assign user to selected role
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    // Role assignment was successful
                    TempData["SuccessMessage"] = $"Role assigned successfully to user {user.UserName}.";
                }
                else
                {
                    // Handle errors
                    TempData["ErrorMessage"] = $"An error occurred while assigning role to user {user.UserName}.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid user or role selected.";
            }

            return RedirectToAction("ManageUserRoles");
        }



    }
    
}
