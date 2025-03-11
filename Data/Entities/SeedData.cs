using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using servicesharing.Data.Entities; // Reference to your custom User class

namespace servicesharing.Data
{
    public static class SeedData
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define admin credentials
            string adminEmail = "adminemail@gmail.com";
            string adminPassword = "Admin123";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create Admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        public static async Task SeedLocationsAsync(AppDbContext context)
        {
            if (!context.Locations.Any()) // Проверка дали вече има записи
            {
                context.Locations.AddRange(new[]
                {
                    new Location
                    {
                        Name = "Автосервиз Мотор-Мастер",
                        Address = "Бургас, ул. Примерна 12",
                        Latitude = 42.5038,
                        Longitude = 27.4626,
                        Details = "Отличен сервиз за двигатели"
                    },
                    new Location
                    {
                        Name = "Автосервиз ДрайвТех",
                        Address = "Бургас, ул. Гоце Делчев 7",
                        Latitude = 42.5050,
                        Longitude = 27.4660,
                        Details = "Специализиран сервиз за скоростни кутии"
                    },
                    new Location
                    {
                        Name = "Автосервиз Октавио",
                        Address = "София, ул. Транспортна 3",
                        Latitude = 42.6977,
                        Longitude = 23.3219,
                        Details = "Опитен екип за ремонт на ходова част"
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
