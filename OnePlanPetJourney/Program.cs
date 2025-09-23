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

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Razor Pages only
            builder.Services.AddRazorPages();

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

            app.MapRazorPages();

            // ✅ fallback must match actual page name
            // Option 1: redirect fallback (safe, no startup validation)
            app.MapFallback(() => Results.Redirect("/Leads/PageOne"));

            // Option 2: strict page fallback (only if the file is guaranteed to exist)
            // app.MapFallbackToPage("/Products/Leads/PageOne");

            app.Run();
        }
    }
}
