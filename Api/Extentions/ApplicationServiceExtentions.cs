using Api.Data;
using Api.Entities.Helpers;
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
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoRepo>();
            services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}