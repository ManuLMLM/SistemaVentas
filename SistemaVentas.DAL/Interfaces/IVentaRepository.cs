using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.Entity;

namespace SistemaVentas.DAL.Interfaces
{
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta venta);
        Task<List<DetalleVenta>> Reporte(DateTime fechaini, DateTime fechafin);
    }
}
