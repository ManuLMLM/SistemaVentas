using SistemaVentas.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using SistemaVentas.Entity;
using SistemaVentas.DAL.Interfaces;

namespace SistemaVentas.BLL.Implementacion
{
    public class FirebaseS : IFirebase
    {
        private readonly IGenericRepository<Configuracion> _repository;
        public FirebaseS(IGenericRepository<Configuracion> repository)
        {
            _repository= repository;
        }
        public async Task<string> SubirStorage(Stream Archivo, string Carpeta, string NombreArchivo)
        {
            var urlima = "";
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(d => d.Recurso.Equals("FireBase_Storage"));
                Dictionary<string, string> Config = query.ToDictionary(keySelector: d => d.Propiedad, elementSelector: d => d.Valor);
                var auto = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auto.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);
                var Tcancel = new CancellationTokenSource();
                var tarea = new FirebaseStorage(Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child(Config[Carpeta]).Child(NombreArchivo).PutAsync(Archivo, Tcancel.Token);
                urlima = await tarea;
              
            }
            catch (Exception)
            {
                urlima = "";
                
            }
            return urlima;
        }
        public async Task<bool> EliminarStorage(string Carpeta, string NombreArchivo)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(d => d.Recurso.Equals("FireBase_Storage"));
                Dictionary<string, string> Config = query.ToDictionary(keySelector: d => d.Propiedad, elementSelector: d => d.Valor);
                var auto = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auto.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);
                var Tcancel = new CancellationTokenSource();
                var tarea = new FirebaseStorage(Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child(Config[Carpeta]).Child(NombreArchivo).DeleteAsync();
                await tarea;
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
