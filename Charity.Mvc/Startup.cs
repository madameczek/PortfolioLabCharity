using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charity.Mvc.Contexts;
using Charity.Mvc.CustomTokenProviders;
using Charity.Mvc.Models;
using Charity.Mvc.Models.DbModels;
using Charity.Mvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Charity.Mvc
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<CharityDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"));
			});
			
			services.AddIdentity<CharityUser, IdentityRole>(config =>
			{
				config.Password.RequiredLength = 4;
				config.Password.RequireDigit = false;
				config.Password.RequireUppercase = false;
				config.Password.RequireNonAlphanumeric = false;
				config.Password.RequireLowercase = false;
				config.SignIn.RequireConfirmedEmail = false;
				config.SignIn.RequireConfirmedPhoneNumber = false;
				config.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
			})
				.AddEntityFrameworkStores<CharityDbContext>()
				.AddDefaultTokenProviders()
				.AddTokenProvider<EmailTokenProvider<CharityUser>>("emailconfirmation");
			services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(2));
			services.Configure<EmailConfirmationTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(3));

			services.AddScoped<SignInManager<CharityUser>>();
			services.AddScoped<UserManager<CharityUser>>();
			services.AddScoped<IDonationService, DonationService>();
			services.AddScoped<IUserManagerService, UserManagerService>();
			services.AddMailKit(config => config.UseMailKit(Configuration.GetSection("MailKitOptions").Get<MailKitOptions>()));
			services.AddControllersWithViews();
			services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));

            // This disables client side validation if needed. 
            // In Chrome, browser validates some fields (like email) anyway.
            /*services.AddRazorPages()
				.AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = false);*/
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
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
				//endpoints.MapRazorPages();
			});
		}
	}
}
