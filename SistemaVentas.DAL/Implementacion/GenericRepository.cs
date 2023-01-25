using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.BaseContext;
using SistemaVentas.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace SistemaVentas.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SistemaVentasContext _dbContext;
        public GenericRepository(SistemaVentasContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<TEntity> Crear(TEntity Entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().AddAsync(Entidad);
                await _dbContext.SaveChangesAsync();
                return Entidad;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Editar(TEntity Entidad)
        {
            try
            {
                _dbContext.Update(Entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Elimiar(TEntity Entidad)
        {
            try
            {
                _dbContext.Remove(Entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> query =filtro == null? _dbContext.Set<TEntity>():
                _dbContext.Set<TEntity>().Where(filtro);
            return query;
        }

    }
}
