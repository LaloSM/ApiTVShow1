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
            // Añade la configuración del contexto de base de datos para ApitvshowContext.
            services.AddDbContext<ApitvshowContext>(options =>
            {
                // Configura el contexto para usar SQL Server con la cadena de conexión llamada "CadenaSQL" desde la configuración.
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            // Registra el servicio de repositorio genérico como Transient.
            // Esto permite que se cree una nueva instancia del repositorio cada vez que se solicita.
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registra el servicio TvShowService como Scoped.
            // Esto asegura que haya una única instancia por solicitud HTTP en una misma sesión de usuario.
            services.AddScoped<ITvShowService, TvShowService>();
        }
    }
}
