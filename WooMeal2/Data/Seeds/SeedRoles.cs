using Microsoft.AspNetCore.Identity;
using WooMeal2.Models;

namespace WooMeal2.Data.Seeds
{
    public class SeedRoles
    {
        public async static Task CreateRoles(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();

                string[] roleNames = { "Admin", "Customer", "RestaurantOwner" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var poweruser = new AppUser
                {
                    UserName = "admin2@gmail.com",
                    Email = "admin2@gmail.com",
                    FirstName = "AD2",
                    LastName = "MIN2",
                    EmailConfirmed= true,
                };

                string adminPWD = "aA1234aaa";

                // RESTAURANT OWNERS

                var McDonaldsPerson = new AppUser
                {
                    UserName = "mcdonalds@mcdonalds.com",
                    Email = "mcdonalds@mcdonalds.com",
                    FirstName = "Mc",
                    LastName = "Do",
                    Type = UserType.Restaurant,
                    EmailConfirmed = true,
                };

                string mcDoPWD = "McDonalds01";

                var PadThaiPerson = new AppUser
                {
                    UserName = "padthai@padthai.com",
                    Email = "padthai@padthai.com",
                    FirstName = "AD2",
                    LastName = "MIN2",
                    Type = UserType.Restaurant,
                    EmailConfirmed = true,
                };

                string padthaiPWD = "PadThai01";

                // ADMIN adding

                var _user = await userManager.FindByEmailAsync("admin2@gmail.com");

                if (_user == null)
                {
                    var createPowerUser = await userManager.CreateAsync(poweruser, adminPWD);
                    if (createPowerUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(poweruser, "Admin");
                    }
                }

                // OWNERS adding

                var _mcdoUser = await userManager.FindByEmailAsync("mcdonalds@mcdonalds.com");

                if (_mcdoUser == null)
                {
                    var createMcDoUser = await userManager.CreateAsync(McDonaldsPerson, mcDoPWD);
                    if (createMcDoUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(McDonaldsPerson, "RestaurantOwner");
                    }
                }

                var _padthaiUser = await userManager.FindByEmailAsync("padthai@padthai.com");

                if (_padthaiUser == null)
                {
                    var createPadThaiUser = await userManager.CreateAsync(PadThaiPerson, padthaiPWD);
                    if (createPadThaiUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(PadThaiPerson, "RestaurantOwner");
                    }
                }
            }
        }
    }
}
