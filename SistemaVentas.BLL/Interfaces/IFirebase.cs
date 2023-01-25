using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Interfaces
{
    public interface IFirebase
    {
        Task<string> SubirStorage(Stream Archivo, string Carpeta, string NombreArchivo);
        Task<bool> EliminarStorage(string Carpeta, string NombreArchivo);
    }
}
