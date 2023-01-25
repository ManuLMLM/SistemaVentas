using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace SistemaVentas.BLL.Implementacion
{
    public class UsuarioS : IUsuarioS
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IFirebase _firebase;
        private readonly IUtilidades _utilidades;
        private readonly ICorreoServicio _correo;
        public UsuarioS(IGenericRepository<Usuario> repository, IFirebase firebase,
            IUtilidades utilidades, ICorreoServicio correo)
        {
            _repository= repository;
            _firebase= firebase;
            _utilidades= utilidades;
            _correo= correo;
        }
        public async Task<List<Usuario>> ListaUsuario()
        {
            IQueryable<Usuario> query = await _repository.Consultar();
            return query.Include(d=>d.IdRolNavigation).ToList();
        }
        public async Task<Usuario> CrearUsuario(Usuario entidad, Stream Foto = null, string Nombrefoto = "", string UrlPlantillaCorreo = "")
        {
            Usuario Uexiste = await _repository.Obtener(d=>d.Correo==entidad.Correo);
            if (Uexiste != null)
                throw new TaskCanceledException("El correo ya existe");
            try
            {
                string clavehecha = _utilidades.GenerarClave();
                entidad.Clave = _utilidades.Convertir256(clavehecha);
                entidad.NombreFoto = Nombrefoto;
                if (Foto !=null)
                {
                    string urlfoto = await _firebase.SubirStorage(Foto,"carpeta_usuario",Nombrefoto);
                    entidad.UrlFoto = urlfoto;
                }
                Usuario usuario_creado = await _repository.Crear(entidad);
                if (usuario_creado.IdUsuario==0)
                    throw new TaskCanceledException("No se pudo crear el usuario");
                if (UrlPlantillaCorreo != "")
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo]", usuario_creado.Correo).Replace("[clave]",clavehecha);
                    string htmlcorreo = "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                    HttpWebResponse response =(HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream data=response.GetResponseStream())
                        {
                            StreamReader readerstream = null;
                            if (response.CharacterSet ==null)
                                readerstream = new StreamReader(data);
                            else
                                readerstream = new StreamReader(data,Encoding.GetEncoding(response.CharacterSet));

                            htmlcorreo = readerstream.ReadToEnd();
                            response.Close();
                            readerstream.Close();
                        }
                    }
                    if (htmlcorreo !="")
                        await _correo.EnviarCorreo(usuario_creado.Correo,"Cuenta Creada",htmlcorreo);                    
                }
                IQueryable<Usuario> query = await _repository.Consultar(u => u.IdUsuario == usuario_creado.IdUsuario);
                usuario_creado = query.Include(r => r.IdRolNavigation).First();
                
                return usuario_creado;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<Usuario> EditarUsuario(Usuario entidad, Stream foto = null, string Nombrefoto = "")
        {
            Usuario Uexiste = await _repository.Obtener(d => d.Correo == entidad.Correo && d.IdUsuario != entidad.IdUsuario);
            if (Uexiste !=null)
                throw new TaskCanceledException("El correo ya existe");
            try
            {
                IQueryable<Usuario> queryusuario = await _repository.Consultar(d => d.IdUsuario == entidad.IdUsuario);
                Usuario usuarioedit = queryusuario.First();

                usuarioedit.Nombre = entidad.Nombre;
                usuarioedit.Correo = entidad.Correo;
                usuarioedit.Telefono = entidad.Telefono;
                usuarioedit.IdRol = entidad.IdRol;

                if (usuarioedit.NombreFoto=="")
                    usuarioedit.NombreFoto = Nombrefoto;

                if (foto!=null)
                {
                    string urlfoto = await _firebase.SubirStorage(foto, "carpeta_usuario",usuarioedit.NombreFoto);
                    usuarioedit.UrlFoto= urlfoto;
                }

                bool respuesta = await _repository.Editar(usuarioedit);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el usuario");

                Usuario usuarioeditado = queryusuario.Include(d=>d.IdRolNavigation).First();
                return usuarioeditado;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> EliminarUsuario(int id)
        {
            try
            {
                Usuario usuarioencontrado = await _repository.Obtener(d => d.IdUsuario == id);
                if (usuarioencontrado==null)
                    throw new TaskCanceledException("El usuario no existe");

                string nombrefoto = usuarioencontrado.NombreFoto;
                bool respuesta = await _repository.Elimiar(usuarioencontrado);
                if (respuesta)
                    await _firebase.EliminarStorage("carpeta_usuario", nombrefoto);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Usuario> ObtenerUsuario(string correo, string clave)
        {
            string clavecensu = _utilidades.Convertir256(clave);
            Usuario usuarioencontrado = await _repository.Obtener(d=>d.Correo.Equals(correo) 
            && d.Clave.Equals(clavecensu));
            return usuarioencontrado;

        }
        public async Task<Usuario> ObtenerId(int id)
        {
            IQueryable<Usuario> query = await _repository.Consultar(d=>d.IdUsuario==id);
            Usuario resultado = query.Include(d => d.IdRolNavigation).FirstOrDefault();
            return resultado;
        }
        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario usuarioencontrado = await _repository.Obtener(d=>d.IdUsuario==entidad.IdUsuario);
                if (usuarioencontrado==null)
                    throw new TaskCanceledException("Usuario no existe");

                usuarioencontrado.Correo = entidad.Correo;
                usuarioencontrado.Telefono = entidad.Telefono;

                bool respuesta = await _repository.Editar(usuarioencontrado);

                return respuesta;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> CambiarClave(int id, string ClaveActual, string ClaveNueva)
        {
            try
            {
                Usuario usuarioencontrado = await _repository.Obtener(d => d.IdUsuario == id);
                if (usuarioencontrado==null)
                    throw new TaskCanceledException("Usuario no existe");

                if (usuarioencontrado.Clave != _utilidades.Convertir256(ClaveActual))
                    throw new TaskCanceledException("La contraseña no es correcta");

                usuarioencontrado.Clave = _utilidades.Convertir256(ClaveNueva);
                bool respuesta = await _repository.Editar(usuarioencontrado);
                return respuesta;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<bool> RecuperarClave(string correo, string url)
        {
            try
            {
                Usuario encontrado = await _repository.Obtener(d => d.Correo == correo);
                if (encontrado == null)
                    throw new TaskCanceledException("No se encontró un usuario con este correo");

                string clavegene = _utilidades.GenerarClave();
                encontrado.Clave = _utilidades.Convertir256(clavegene);

                url = url.Replace("[clave]", clavegene);
                string htmlcorreo = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        StreamReader readerstream = null;
                        if (response.CharacterSet == null)
                            readerstream = new StreamReader(data);
                        else
                            readerstream = new StreamReader(data, Encoding.GetEncoding(response.CharacterSet));

                        htmlcorreo = readerstream.ReadToEnd();
                        response.Close();
                        readerstream.Close();
                    }
                }
                bool correoenviado = false;
                if (htmlcorreo != "")
                   correoenviado = await _correo.EnviarCorreo(correo, "Contraseña reestablecida", htmlcorreo);

                if (correoenviado==false)
                    throw new TaskCanceledException("Tenemos problemas. Por favor intente de nuevo más tarde");

                bool respuesta = await _repository.Editar(encontrado);
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
