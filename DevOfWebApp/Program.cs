using DevOfWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DevOfWebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<LocalDBContext>();
			builder.Services.AddControllersWithViews();
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => options.LoginPath = "/login");

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Main}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "briefDefault",
                pattern: "/{action=Index}/{id?}",
				new {controller = "Main"});

            app.Run();
		}
	}
}