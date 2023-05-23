using Api.Data;
using Api.Entities.Interfaces;
using Api.Entities.Repo;
using Microsoft.EntityFrameworkCore;

namespace Api.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                         IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
             opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}