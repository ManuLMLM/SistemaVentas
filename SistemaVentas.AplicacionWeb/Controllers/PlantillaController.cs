﻿using Microsoft.AspNetCore.Mvc;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class PlantillaController : Controller
    {
        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"]=correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }
        public IActionResult RecuperarClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }
    }
}
