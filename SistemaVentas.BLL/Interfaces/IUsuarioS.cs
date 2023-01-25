using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.BLL.Interfaces
{
    public interface IUsuarioS
    {
        Task<List<Usuario>> ListaUsuario();
        Task<Usuario> CrearUsuario(Usuario entidad,Stream foto=null, string Nombrefoto=""
            ,string UrlPlantillaCorreo = "");
        Task<Usuario> EditarUsuario(Usuario entidad, Stream foto = null, string Nombrefoto = "");
        Task<bool> EliminarUsuario(int id);
        Task<Usuario> ObtenerUsuario(string correo, string clave);
        Task<Usuario> ObtenerId(int id);
        Task<bool> GuardarPerfil(Usuario entidad);
        Task<bool> CambiarClave(int id,string ClaveActual,string ClaveNueva);
        Task<bool> RecuperarClave(string correo,string url);

    }
}
