using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace RuanganKita.Api.Helper;

public static class AuthHelper
{
    public static IServiceCollection BuildJWT(
        this IServiceCollection services)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new Exception("JWT_KEY is not set.");

        if (string.IsNullOrWhiteSpace(jwtIssuer))
            throw new Exception("JWT_ISSUER is not set.");

        if (string.IsNullOrWhiteSpace(jwtAudience))
            throw new Exception("JWT_AUDIENCE is not set.");

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
