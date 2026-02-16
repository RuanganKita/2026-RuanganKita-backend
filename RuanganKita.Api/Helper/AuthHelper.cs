using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RuanganKita.Api.Helper;

public static class AuthHelper
{
    public static IServiceCollection BuildJWT(
        this IServiceCollection services)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var frontendPort = Environment.GetEnvironmentVariable("FRONTEND_PORT");

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new Exception("JWT_KEY is not set.");

        if (string.IsNullOrWhiteSpace(jwtIssuer))
            throw new Exception("JWT_ISSUER is not set.");

        if (string.IsNullOrWhiteSpace(jwtAudience))
            throw new Exception("JWT_AUDIENCE is not set.");

        if (string.IsNullOrWhiteSpace(frontendPort))
            throw new Exception("FRONTEND_PORT is not set.");

        services.AddCors(options =>
        {
            options.AddPolicy("FrontendPolicy", policy =>
            {
                policy.WithOrigins(frontendPort)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)
                        ),
                };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("VerifiedAdmin", policy =>
                policy.RequireClaim("isVerified", "true")
                    .RequireRole("Admin"));
        });

        return services;
    }
}
