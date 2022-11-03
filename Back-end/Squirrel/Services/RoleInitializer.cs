using Microsoft.AspNetCore.Identity;
using Squirrel.Entities;

namespace Squirrel.Services
{
    public class RoleInitializer
    {
        public static async Task RoleInit(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            string adminEmail = configuration["Admin:Email"];
            string password = configuration["Admin:Password"];

            if (await roleManager.FindByNameAsync("admin") is null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("user") is null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await roleManager.FindByNameAsync("premium") is null)
                await roleManager.CreateAsync(new IdentityRole("premium"));

            if (await userManager.FindByEmailAsync(adminEmail) is null)
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
