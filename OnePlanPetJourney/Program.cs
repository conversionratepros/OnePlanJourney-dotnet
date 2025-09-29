using Microsoft.EntityFrameworkCore;
using OnePlanPetJourney.Data;

namespace OnePlanPetJourney
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Db + Razor Pages
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddRazorPages();

            // ✅ Session requires a cache + must be registered BEFORE Build()
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".OnePlanPetJourney.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ✅ If you have AlertController or any MVC controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ✅ Session must be before endpoint mapping
            app.UseSession();

            // (Authentication/Authorization here if used)
            // app.UseAuthentication();
            // app.UseAuthorization();

            // ✅ Map controllers (needed if you created AlertController)
            app.MapControllers();

            // Razor Pages
            app.MapRazorPages();

            // Fallback to your first page
            app.MapFallback(() => Results.Redirect("/Leads/PageOne"));

            app.Run();
        }
    }
}
