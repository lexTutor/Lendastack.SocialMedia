using Application.Core.Entities;
using Application.Infrastructure.Data.DbContext;
using Application.Infrastructure.Data.Repository;
using Application.Infrastructure.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Infrastructure;

public static class InfrastructureInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(ILogManager<>), typeof(LogManager<>));
    }

    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<AppDbContext>(option =>
        {
            option.UseSqlServer(connectionString);
        });
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Email confirmation is disabled for easier testing to avoid sending emails which is out of scope.
            //Complex password validations are disabled for easier testing.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtConfiguration:Issuer"],
                ValidAudience = Configuration["JwtConfiguration:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfiguration:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();
    }
}
