using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CarShare.Data;
using Microsoft.AspNetCore.Identity;
using CarShare.Areas.Identity.Data;
using CarShare.Models;
using CarShare.Services;

namespace CarShare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CarShareContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CarShareContext") ?? throw new InvalidOperationException("Connection string 'CarShareContext' not found.")));

            builder.Services.AddDefaultIdentity<CarShareUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CarShareContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //builder.Services.AddSingleton<CarShareContext>();

            //builder.Services.AddScoped<IBackgroundTaskRunner, BackgroundTaskRunner>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CarShareUser>>();

                var roles = new[] { "Client", "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string email = "admin@admin.com";
                string password = "Admin1234%";

                

                if(await userManager.FindByEmailAsync(email) == null)
                {
                    var address = new Adress();
                    address.City = "adminowo";
                    address.Street = "adminowa";
                    address.Number = 1;
                    address.ZipNumber = "11-111";
                    var client = new Client();
                    client.FirstName = "admin";
                    client.LastName = "admin";
                    client.ClientAdress = address;
                    var user = new CarShareUser();
                    user.UserName = "Admin";
                    user.Email = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //var backgroundTaskRunner = app.Services.GetRequiredService<IBackgroundTaskRunner>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            app.MapRazorPages();
            app.Run();
        }
    }
}