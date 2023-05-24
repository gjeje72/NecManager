namespace NecManager.Server.Api.Security;

using System;
using System.Globalization;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using NecManager.Common.Security;
using NecManager.Server.Api.Business.TokenProviders;
using NecManager.Server.DataAccessLayer.EntityLayer;
using NecManager.Server.DataAccessLayer.Model;

public static class SecurityExtension
{
    /// <summary>
    ///     Set security definition for API.
    /// </summary>
    /// <param name="services">service collection from DI.</param>
    /// <param name="configuration">Configuration provider from DI.</param>
    /// <returns>services collection.</returns>
    public static IServiceCollection AddSecurityDefinition(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<Student, IdentityRole>(opt =>
        {
            opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            opt.SignIn.RequireConfirmedAccount = false;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.SignIn.RequireConfirmedPhoneNumber = false;
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequiredLength = 6;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<NecLiteDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<Student>>("emailconfirmation");

        services.AddAuthentication(options =>
        {
            // Specify that the default auth scheme should be 'bearer' as we are using JWT for authentification
            // For example API should return a 401 code there no or incorrect token in the header
            // However a website would redirect to a login page, which, for an API, does not exists and would result in a 404 code instead of 401
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        // We also must provide information on how the server should verify a bearer token
        .AddJwtBearer(parameters =>
        {
            parameters.RequireHttpsMetadata = false;
            parameters.SaveToken = true;
            parameters.TokenValidationParameters = new TokenValidationParameters // those are use to define how the token is validated
            {
                ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecurityKey"))),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(int.Parse(configuration.GetValue<string>("Jwt:ExpiryInSeconds"), CultureInfo.InvariantCulture))
            };
        });

        services.AddAuthorization(CommonAuthorizationHelper.PoliciesOptions);

        services.AddScoped<UserAuthorizationContext>();

        return services;
    }
}
