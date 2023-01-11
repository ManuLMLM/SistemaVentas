using Microsoft.AspNetCore.Mvc;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class ReporteVentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
