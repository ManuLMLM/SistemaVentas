using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.DAL;

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
        }
    }
}
