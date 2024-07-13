using Microsoft.OpenApi.Models;

namespace BookyApp.Helper
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Booky",
                    Version = "v1"
                });

                var security = new Dictionary<string, IEnumerable<string>>
             {
                 { "Bearer", new string[0] }
             };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWt Auth using bearer scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 { new OpenApiSecurityScheme { Reference = new OpenApiReference
                     {
                         Id = "Bearer",
                         Type = ReferenceType.SecurityScheme
                     }
                 }, new List<string>() }
             });

            });

            return services;
        }
    }
}
