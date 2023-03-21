using FinalProjectMVC.Constants;
using Microsoft.AspNetCore.Identity;
using System;

namespace FinalProjectMVC.Areas.Identity.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles

            // 1- getting the services (API's) responsible for `Users` and `roles` manging
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 2- creating and adding roles to the `roleManger` Instance
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Seller.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.CustomerService.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.WebDeveloper.ToString()));


            // creating admin in code to be secure. 

            var user = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "Raoufalaadin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);


            // If no user is found with that email (Email = "admin@gmail.com")
            // Then we create an new account and set it as admin.

            // This part ensures that there is always an admin account.
            if (userInDb == null)
            {
                // setting it's password
                await userManager.CreateAsync(user, "Admin@123");
                // setting it's role
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }

            /* If the email does exist, we will assing the role in the 
             register.cs for some reason ====> search !! 
            
             */
        }

    }
}
