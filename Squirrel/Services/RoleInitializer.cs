using Microsoft.AspNetCore.Identity;
using Squirrel.Entities;

namespace Squirrel.Services
{
    public class RoleInitializer
    {
        public static async Task RoleInit(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@nextgenmail.com";
            string password = "_Aa123456";

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await roleManager.FindByNameAsync("service") == null)
                await roleManager.CreateAsync(new IdentityRole("service"));

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new() { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.ConfirmEmailAsync(admin!, await userManager.GenerateEmailConfirmationTokenAsync(admin));
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
