using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data;
using servicesharing.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var context = services.GetRequiredService<AppDbContext>();
    await SeedData.SeedRolesAsync(roleManager);
    await SeedData.SeedAdminUserAsync(userManager);
    await SeedData.SeedLocationsAsync(context);
    await SeedData.SeedServicesAsync(context);
    await SeedData.SeedPromotionsAsync(context, userManager);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Add a new class for seeding data
public static class SeedData
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var adminEmail = "adminemail@gmail.com";
        var adminPassword = "Admin123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "Administrator", // Ensure FullName is populated
                Role = "Admin"
            };

            var result = await userManager.CreateAsync(newUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
            }
            else
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
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
    public static async Task SeedLocationsAsync(AppDbContext context)
    {
        var locations = new[]
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
    };

        foreach (var location in locations)
        {
            if (!context.Locations.Any(l => l.Name == location.Name && l.Address == location.Address))
            {
                context.Locations.Add(location);
            }
        }

        await context.SaveChangesAsync();
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