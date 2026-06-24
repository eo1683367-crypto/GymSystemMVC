using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.Utilities;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Repositories.Classes;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //DI
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


           builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));

            #region Register Repositories And UOW

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>(); 
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            #endregion

            // Map Configuration
            builder.Services.AddAutoMapper(m => m.AddProfile(new ProjectProfile()));
            #region Register Services

            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IMemberShipService, MemberShipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>(); 

            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets(); // To Read Files
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
