using B2BPriceAdmin.API.CustomTokenProviders;
using B2BPriceAdmin.API.Extensions;
using B2BPriceAdmin.Core.Authorization;
using B2BPriceAdmin.Database;
using B2BPriceAdmin.DTO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace B2BPriceAdmin.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }
        private const string _defaultCorsPolicyName = "DefaultCORSPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(validations =>
            {
                validations.RegisterValidatorsFromAssemblyContaining<LoginDTOValidator>();
                validations.DisableDataAnnotationsValidation = true;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddExceptional(Configuration.GetSection("Exceptional"));
            services.AddHttpContextAccessor();
            services.AddDbContext<B2BPriceDbContext>(options => options.UseSqlServer("name=Exceptional:Store:ConnectionString"));
            ServiceExtension.ConfigureDependencyInjection(services);
            // Permission-based authorization middleware
            // Reference Article: https://www.zehntec.com/blog/permission-based-authorization-in-asp-net-core/
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            ServiceExtension.ConfigureSwagger(services);
            services.AddSwaggerGen();
            ServiceExtension.ConfigureIdentity(services);
            services.Configure<Microsoft.AspNetCore.Identity.DataProtectionTokenProviderOptions>
                (opt =>
            opt.TokenLifespan = TimeSpan.FromMinutes(5)
            );
            services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromMinutes(5));

            ServiceExtension.ConfigureJWTAuthentication(services, Configuration);
            ServiceExtension.ConfigureCORS(services, Configuration, _defaultCorsPolicyName);
        }

        public void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment webHostEnvironment)
        {
            applicationBuilder.UseSwagger(c =>
            {
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });

            applicationBuilder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/v1/swagger.json", "PlaceInfo Services");
                c.RoutePrefix = string.Empty;
            });

            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseExceptionHandler("/error-local-development");
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/error");
            }
            applicationBuilder.UseExceptional();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseCors(_defaultCorsPolicyName);
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }
}
