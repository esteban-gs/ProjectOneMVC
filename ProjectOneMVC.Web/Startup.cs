using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ProjectOneMVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Newtonsoft.Json;
using ProjectOneMVC.Core.Entities;
using ProjectOneMVC.Data.Data.Initialization;
using ProjectOneMVC.Data.Data.Repository;
using System.Net.Mime;
using ProjectOneMVC.Web.Services;

namespace ProjectOneMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = false
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Home/Login";
            });

            // Repositories
            services.AddScoped<ClassRepository>();
            services.AddScoped<UserClassRepository>();

            // Service
            services.AddScoped<ClassService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            IdentityResult roleResult;
            //here in this line we are adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //here in this line we are creating admin role and seed it to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //here we are assigning the Admin role to the User that we have registered above 
            //Now, we are assinging admin role to this user("Ali@gmail.com"). When will we run this project then it will
            //be assigned to that user.
            IdentityUser user = await UserManager.FindByEmailAsync("este@example.com");
            var userExists = user != null;
            if (!userExists)
            {
                var newUser = new IdentityUser("este@example.com") { Email = "este@example.com" };
                await UserManager.CreateAsync(newUser, "Secret123$");
                IdentityUser createdUser = await UserManager.FindByEmailAsync("este@example.com");
                await UserManager.AddToRoleAsync(createdUser, "Admin");
            }
            if (userExists && !await UserManager.IsInRoleAsync(user, "Admin"))
            {
                await UserManager.AddToRoleAsync(user, "Admin");
            }

            if (!context.Classes.Any())
            {
                var json = SeedData.ClassJson;
                var classes = JsonConvert.DeserializeObject<List<Class>>(json);
                foreach (var record in classes)
                {
                    context.Classes.Add(record);
                    context.SaveChanges();
                }
            }
        }
    }
}
