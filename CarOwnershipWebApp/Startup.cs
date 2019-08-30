using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarOwnershipWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CarOwnershipWebApp.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using CarOwnershipWebApp.Models;
using Microsoft.Extensions.Logging;

namespace CarOwnershipWebApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var dbSettings = Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

            string connectionString = Configuration.GetConnectionString("HerokuConnection");
            connectionString = connectionString.Replace("{host}", dbSettings.Host);
            connectionString = connectionString.Replace("{port}", dbSettings.Port);
            connectionString = connectionString.Replace("{database}", dbSettings.Database);
            connectionString = connectionString.Replace("{username}", dbSettings.User);
            connectionString = connectionString.Replace("{password}", dbSettings.Password);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlite(
            //        Configuration.GetConnectionString("SqliteConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                //works only on HTTPS (heroku does not have free https)
                //.AddFacebook(facebookOptions => 
                //{
                //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                //})
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });


            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<EndpointSettings>(Configuration.GetSection("EndpointSettings"));
            services.Configure<EUpravaSettings>(Configuration.GetSection("EUpravaSettings"));

            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IOutcallsService, OutcallService>();
            services.AddScoped<IBlockchainService, BlockchainService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //CreateUserRoles(serviceProvider).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;
            //Adding Addmin Role  
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Assign Admin role to the main User here we have given our newly loregistered login id for Admin management  
            IdentityUser user = await UserManager.FindByEmailAsync("gasperinmatevz@gmail.com");
            var User = new IdentityUser();
            await UserManager.AddToRoleAsync(user, "Admin");

        }
    }
}
