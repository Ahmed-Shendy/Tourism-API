using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Tourism_Api.model.Context;
using Tourism_Api.Services;

namespace Tourism_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();


            // Add Hangfire services
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));

            // Add Hangfire server
            builder.Services.AddHangfireServer(options =>
            {
                options.ServerName = "Tourism.API.Hangfire";
                options.WorkerCount = 1;
            });

            // Add other services (Dependencies, DbContext, etc.)
            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddDbContext<TourismContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register BookingExpirationService
            builder.Services.AddSingleton<BookingExpirationService>();

            // Configure Serilog
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                });

            var app = builder.Build();

            // Enable Swagger in all environments
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tourism API v1");
                c.RoutePrefix = "swagger";
            });

            // Configure Hangfire dashboard (secured)
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Tourism API Jobs Dashboard",
                Authorization = new[] { new HangfireAuthorizationFilter() },
                StatsPollingInterval = 5000
            });

            // Schedule the booking expiration job
            RecurringJob.AddOrUpdate<BookingExpirationService>(
                "expire-bookings",
                x => x.CheckAndExpireBookings(),
                "0 0 * * *"); // تشغيل كل ساعة

            // Middleware pipeline
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.UseExceptionHandler();
            app.MapControllers();
            app.UseStaticFiles();
            app.MapStaticAssets();

            app.Run();
        }
    }

    // Hangfire authorization filter (يمكن تعديله حسب نظام المصادقة الخاص بك)
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // في بيئة الإنتاج، يجب تطبيق المصادقة المناسبة هنا
            var httpContext = context.GetHttpContext();

            // للتنمية فقط - للسماح للجميع
            return true;

            // للإنتاج، استخدم شيء مثل:
            // return httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("Admin");
        }
    }
}