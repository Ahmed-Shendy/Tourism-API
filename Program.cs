
using Microsoft.EntityFrameworkCore;
using Serilog;
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

            // Configure Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Tourism API",
                    Version = "v1",
                    Description = "API for Tourism Management"
                });
            });

            // Add other services (Dependencies, DbContext, etc.)
            builder.Services.AddDependencies(builder.Configuration);

            builder.Services.AddDbContext<TourismContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Serilog
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            // Enable Swagger in all environments (or use IsDevelopment() for dev-only)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tourism API v1");
                // c.RoutePrefix = "swagger"; // Default is "swagger" (optional)
            });

            // Middleware pipeline
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.UseExceptionHandler();
            app.MapControllers();
            app.MapStaticAssets(); // For .NET 9

            app.Run();
        }
    }
}
