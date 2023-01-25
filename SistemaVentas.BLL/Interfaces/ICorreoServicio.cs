using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Interfaces
{
    public interface ICorreoServicio
    {
        Task<bool> EnviarCorreo(string Cdestino, string Casunto,string Cmensaje);
    }
}
