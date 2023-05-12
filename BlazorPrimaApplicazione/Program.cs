using BlazorPrimaApplicazione.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using ServiceStack.OrmLite;
using Sotsera.Blazor.Toaster.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorPrimaApplicazione
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var connectionString = builder.Configuration.GetConnectionString("PizzaContextConnection") ?? throw new InvalidOperationException("Connection string 'PizzaContextConnection' not found.");

            //builder.Services.AddDbContext<PizzaContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<PizzaContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PizzaContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddToaster(config =>
            {
                //example customizations
                config.PositionClass = Defaults.Classes.Position.TopRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = false;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapRazorPages();
            app.MapFallbackToPage("/_Host");

            RunDbMigrations();

            app.Run();
        }

        private static void RunDbMigrations()
        {
            using var db = OrmHelper.OpenConnection();
            db.CreateMissingColumns<Pizza>(); // Idempotent - running the same thing twice doesn't matter if Pizza stays the same
        }
    }
}