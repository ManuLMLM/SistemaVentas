using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Interfaces
{
    public interface IUtilidades
    {
        string GenerarClave();
        string Convertir256(string texto);
    }
}
