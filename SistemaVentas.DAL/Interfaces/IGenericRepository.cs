using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace SistemaVentas.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>>filtro);
        Task<TEntity> Crear(TEntity Entidad);
        Task<bool> Editar(TEntity Entidad);
        Task<bool> Elimiar(TEntity Entidad);
        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro=null);
    }
}
