using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity;

namespace SistemaVentas.BLL.Implementacion
{
    public class RolesS : IRoles
    {
        private readonly IGenericRepository<Rol> _repository;
        public RolesS(IGenericRepository<Rol> repository)
        {
            _repository= repository;
        }
        public async Task<List<Rol>> lista()
        {
            IQueryable<Rol> query = await _repository.Consultar();
            return query.ToList();
        }
    }
}
