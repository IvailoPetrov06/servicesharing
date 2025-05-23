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
    await SeedData.SeedMechanicRoleOnlyAsync(roleManager);
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
    public static async Task SeedMechanicRoleOnlyAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("�����������"))
        {
            await roleManager.CreateAsync(new IdentityRole("�����������"));
        }
    }
    public static async Task SeedPromotionsAsync(AppDbContext context, UserManager<User> userManager)
    {
        // ������� ��������������, ����� ���������� ��-����
        var admin = await userManager.FindByEmailAsync("adminemail@gmail.com");
        if (admin == null) return;

        // ������� ������� ������ �� ������
        var service1 = context.Services.FirstOrDefault(s => s.Name.Contains("�����"));
        var service2 = context.Services.FirstOrDefault(s => s.Name.Contains("����"));

        if (service1 == null || service2 == null) return;

        // ����������� ���� ���� ��� ��������
        if (!context.Promotions.Any())
        {
            context.Promotions.AddRange(new[]
            {
            new Promotion
  {
      Title = "�������� �� ����� �� �����",
      Description = "������� 20% ��������� �� ���� �� ������!",
      ValidUntil = DateTime.Now.AddMonths(1),
      ServiceId = service1.Id,
      UserId = admin.Id
  },
  new Promotion
  {
      Title = "�������� �� ����� �� ����",
      Description = "����� 15% ��������� ��� ����� �� ����!",
      ValidUntil = DateTime.Now.AddMonths(2),
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
            Name = "���������� �����-������",
            Address = "������, ��. �������� 12",
            Latitude = 42.5038,
            Longitude = 27.4626,
            Details = "������� ������ �� ���������"
        },
        new Location
        {
            Name = "���������� ��������",
            Address = "������, ��. ���� ������ 7",
            Latitude = 42.5050,
            Longitude = 27.4660,
            Details = "������������� ������ �� ��������� �����"
        },
        new Location
        {
            Name = "���������� �������",
            Address = "�����, ��. ����������� 3",
            Latitude = 42.6977,
            Longitude = 23.3219,
            Details = "������ ���� �� ������ �� ������ ����"
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
            new Service { Name = "����� �� �������� �������", Price = 80 },
            new Service { Name = "����� �� ��������", Price = 50 },
            new Service { Name = "����������� �� ABS", Price = 40 },

            new Service { Name = "����� �� �����", Price = 120 },
            new Service { Name = "����� �� ������� ������", Price = 60 },
            new Service { Name = "�������� �� ������������� �������", Price = 80 },

            new Service { Name = "���������� �����������", Price = 50 },
            new Service { Name = "�������� �� �����������", Price = 20 },
            new Service { Name = "�������� �� ����������", Price = 30 },

            new Service { Name = "����� �� ����� � ������ ������", Price = 70 },
            new Service { Name = "����� �� �������� ������", Price = 30 },
            new Service { Name = "�������� � ����� �� ��������", Price = 60 },

            new Service { Name = "����� �� ����", Price = 30 },
            new Service { Name = "������ �� ����", Price = 25 },
            new Service { Name = "������ �� ������� ����", Price = 20 },

            new Service { Name = "����� �� �����������", Price = 100 },
            new Service { Name = "����� �� �������", Price = 80 },
            new Service { Name = "����� �� �������� ����������", Price = 90 }
        });

            await context.SaveChangesAsync();
        }
    }
}