using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.DAL.Interfaces;
using SistemaVentas.Entity;
namespace SistemaVentas.BLL.Implementacion
{
    public class CorreoServicio : ICorreoServicio
    {
        private readonly IGenericRepository<Configuracion> _repository;
        public CorreoServicio(IGenericRepository<Configuracion> repository)
        {
            _repository= repository;
        }
        public async Task<bool> EnviarCorreo(string Cdestino, string Casunto, string Cmensaje)
        {
            try
            {
                IQueryable<Configuracion> query = await _repository.Consultar(d => d.Recurso.Equals("Servicio_Correo"));
                Dictionary<string, string> Config = query.ToDictionary(keySelector: d => d.Propiedad, elementSelector: d => d.Valor);
                var credencial = new NetworkCredential(Config["correo"], Config["clave"]);
                var correo = new MailMessage()
                {
                    From = new MailAddress(Config["correo"], Config["alias"]),
                    Subject = Casunto,
                    Body = Cmensaje,
                    IsBodyHtml = true
                };
                correo.To.Add(new MailAddress(Cdestino));
                var ClientServer = new SmtpClient()
                {
                    Host = Config["host"],
                    Port = int.Parse(Config["puerto"]),
                    Credentials=credencial,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl= true  
                };
                ClientServer.Send(correo);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
