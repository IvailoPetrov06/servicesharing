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
        public static async Task SeedPromotionsAsync(AppDbContext context, UserManager<User> userManager)
        {
            // Вземаме администратора, който създадохме по-рано
            var admin = await userManager.FindByEmailAsync("adminemail@gmail.com");
            if (admin == null) return;

            // Вземаме няколко услуги от базата
            var service1 = context.Services.FirstOrDefault(s => s.Name.Contains("масло"));
            var service2 = context.Services.FirstOrDefault(s => s.Name.Contains("гуми"));

            if (service1 == null || service2 == null) return;

            // Проверяваме дали вече има промоции
            if (!context.Promotions.Any())
            {
                context.Promotions.AddRange(new[]
                {
            new Promotion
            {
                Title = "Отстъпка за смяна на масло",
                Description = "Вземете 20% намаление до края на месеца!",
                ValidUntil = DateTime.Now.AddMonths(1),
                ServiceId = service1.Id,
                UserId = admin.Id
            },
            new Promotion
            {
                Title = "Безплатен монтаж на гуми",
                Description = "Вземи безплатен монтаж при смяна на 4 гуми!",
                ValidUntil = DateTime.Now.AddDays(10),
                ServiceId = service2.Id,
                UserId = admin.Id
            }
        });

                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedServicesAsync(AppDbContext context)
        {
            if (!context.Services.Any())
            {
                context.Services.AddRange(new[]
                {
            new Service { Name = "Смяна на спирачни дискове", Price = 80 },
            new Service { Name = "Смяна на накладки", Price = 50 },
            new Service { Name = "Диагностика на ABS", Price = 40 },

            new Service { Name = "Смяна на масло", Price = 120 },
            new Service { Name = "Смяна на горивен филтър", Price = 60 },
            new Service { Name = "Проверка на инжекционната система", Price = 80 },

            new Service { Name = "Компютърна диагностика", Price = 50 },
            new Service { Name = "Проверка на акумулатора", Price = 20 },
            new Service { Name = "Проверка на спирачките", Price = 30 },

            new Service { Name = "Смяна на масло и маслен филтър", Price = 70 },
            new Service { Name = "Смяна на въздушен филтър", Price = 30 },
            new Service { Name = "Проверка и смяна на антифриз", Price = 60 },

            new Service { Name = "Смяна на гуми", Price = 30 },
            new Service { Name = "Баланс на гуми", Price = 25 },
            new Service { Name = "Ремонт на спукана гума", Price = 20 },

            new Service { Name = "Смяна на амортисьори", Price = 100 },
            new Service { Name = "Смяна на пружини", Price = 80 },
            new Service { Name = "Смяна на кормилни накрайници", Price = 90 }
        });

                await context.SaveChangesAsync();
            }
        }
    }

}
