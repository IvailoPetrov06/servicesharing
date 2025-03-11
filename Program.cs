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