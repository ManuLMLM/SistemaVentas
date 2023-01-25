using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.DAL.BaseContext;
using SistemaVentas.DAL.Implementacion;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.BLL.Implementacion;
using SistemaVentas.BLL.Interfaces;

namespace SistemaVentas.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SistemaVentasContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConectionSQL"));
            }
            );
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository,VentaRepository>();
            services.AddScoped<ICorreoServicio, CorreoServicio>();
            services.AddScoped<IFirebase, FirebaseS>();
            services.AddScoped<IUtilidades, UtilidadesS>();
            services.AddScoped<IRoles, RolesS>();
            services.AddScoped<IUsuarioS, UsuarioS>();
        }
    }
}
