using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities.Interfaces;
using Api.Entities.Repo;
using Microsoft.EntityFrameworkCore;

namespace Api.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(IServiceCollection services,
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