using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.BLL.Interfaces;
using System.Security.Cryptography;

namespace SistemaVentas.BLL.Implementacion
{
    public class UtilidadesS : IUtilidades
    {
        public string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0,6);
            return clave;

        }
        public string Convertir256(string texto)//Método para encriptar una contraseña
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enco = Encoding.UTF8;
                byte[] resultado = hash.ComputeHash(enco.GetBytes(texto));
                foreach (byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
