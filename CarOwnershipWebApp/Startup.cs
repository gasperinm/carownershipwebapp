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

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddFacebook(facebookOptions => 
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOptions => 
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });


            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<IEmailSender, EmailService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
        }
    }
}
