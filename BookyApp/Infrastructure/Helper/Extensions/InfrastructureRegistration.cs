using Domain.Entities;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Helper.Extensions
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
               
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
             
              .AddEntityFrameworkStores<AppDbContext>();

            //        services.AddIdentityCore<ApplicationUser, IdentityRole>()
            //.AddEntityFrameworkStores<AppDbContext>();







            return services;
        }
    }
}
