using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShow.Bll.Implementacion;
using TvShow.Bll.Interfaces;
using TvShow.Dal.Implementacion;
using TvShow.Dal.Interfaces;
using TvShow.Entity;

namespace TvShow.IOC
{
    public static class Dependencias
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            //Añadimos la referencia de la conexion
            services.AddDbContext<ApitvshowContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITvShowService, TvShowService>();
        }
    }
}
