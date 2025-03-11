using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Tourism_Api.model;
using Tourism_Api.model.Context;
using Tourism_Api.Outherize;
using Tourism_Api.Services;
using Tourism_Api.Services;
using Tourism_Api.Services.IServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization;
namespace Tourism_Api;

public static class Depandence
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddAuthConfig(configuration);
        
        services.AddScoped<IAuthenticatServices , AuthenticatServices>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IAdminServices, AdminServices>();

        services.AddScoped<token>();
        services.AddScoped<TourismContext>();
        services.AddIdentity<User, UserRole>()
            .AddEntityFrameworkStores<TourismContext>()
            .AddDefaultTokenProviders();



        // تسجيل FluentValidation

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        // use HybridCache
        services.AddHybridCache();

        

        return services;
    }

    private static IServiceCollection AddAuthConfig
        (this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience
            };
        });


        // identity satting
        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;


            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;

                

            //options.User.RequireUniqueEmail = true;
        });
        return services;
    }

}
