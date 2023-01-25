using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.DAL.BaseContext;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity;
namespace SistemaVentas.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly SistemaVentasContext _dbContext;
        public VentaRepository(SistemaVentasContext dbContext): base (dbContext)
        {
            _dbContext= dbContext;
        }
        public async Task<Venta> Registrar(Venta venta)
        {
            Venta VentaHecha = new Venta();
            using (var transa = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in venta.DetalleVenta)
                    {
                        Producto productoesta = _dbContext.Productos.Where(d => d.IdProducto == dv.IdProducto).First();
                        productoesta.Stock = productoesta.Stock - dv.Cantidad;
                        _dbContext.Productos.Update(productoesta);
                    }
                    await _dbContext.SaveChangesAsync();
                    NumeroCorrelativo numcorre = _dbContext.NumeroCorrelativos.Where(d => d.Gestion == "venta").First();
                    numcorre.UltimoNumero = numcorre.UltimoNumero + 1;
                    numcorre.FechaActualizacion = DateTime.Now;
                    _dbContext.NumeroCorrelativos.Update(numcorre);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0",numcorre.CantidadDigitos.Value));
                    string numventa = ceros + numcorre.UltimoNumero.ToString();
                    numventa = numventa.Substring(numventa.Length-numcorre.CantidadDigitos.Value,numcorre.CantidadDigitos.Value);

                    venta.NumeroVenta = numventa;

                    await _dbContext.Venta.AddAsync(venta);
                    await _dbContext.SaveChangesAsync();

                    VentaHecha = venta;
                    transa.Commit();
                }
                catch (Exception ex)
                {
                    transa.Rollback();
                    throw ex;
                }
            }
            return VentaHecha;
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime fechaini, DateTime fechafin)
        {
            List<DetalleVenta> listaResu = await _dbContext.DetalleVenta
                .Include(d => d.IdVentaNavigation).ThenInclude(d => d.IdUsuarioNavigation)
                .Include(d => d.IdVentaNavigation).ThenInclude(d => d.IdTipoDocumentoVentaNavigation)
                .Where(d => d.IdVentaNavigation.FechaRegistro.Value.Date >= fechaini.Date &&
                d.IdVentaNavigation.FechaRegistro.Value.Date <= fechafin.Date).ToListAsync();
            return listaResu;
        }
    }
}
