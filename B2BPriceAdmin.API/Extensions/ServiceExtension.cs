using System.Text;
using AutoMapper;
using B2BPriceAdmin.API.CustomTokenProviders;
using B2BPriceAdmin.Common.Extensions;
using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.Core.Services;
using B2BPriceAdmin.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace B2BPriceAdmin.API.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Configure Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            // Scoped (Services)
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        /// <summary>
        /// Configure Swagger for open api
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(IServiceCollection services)
        {
            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "B2B Price Admin API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                      new OpenApiSecurityScheme
                        {
                          Reference = new OpenApiReference
                            {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                             }
                            },
                        Array.Empty<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Configure JWT Based Auth
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection("JwtBearer");
            var SecurityKey = appSettingsSection["SecurityKey"];
            var secretKey = Encoding.UTF8.GetBytes(SecurityKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = appSettingsSection["Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = appSettingsSection["Issuer"],

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = appSettingsSection["Audience"],

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// Configure CORS settings.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_defaultCorsPolicyName"></param>
        public static void ConfigureCORS(this IServiceCollection services, IConfiguration Configuration, string _defaultCorsPolicyName)
        {
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                    .WithOrigins(
                        // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                        Configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    )
);
        }

        /// <summary>
        /// Configure Automapper Profiles.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMapping(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(map =>
            {
                //map.AddProfile<>();
                //map.AddProfile<>();
                //map.AddProfile<>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        /// <summary>
        /// Configure Identity Options.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<Database.Entities.User, Database.Entities.Role>(o =>
            {
                o.SignIn.RequireConfirmedAccount = true;
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = true;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.AllowedForNewUsers = true;
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Tokens.EmailConfirmationTokenProvider = "Email";
            }).AddRoles<Database.Entities.Role>()
            .AddEntityFrameworkStores<B2BPriceDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<Database.Entities.User>>("Email");
        }
    }
}
