using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SistemaVentas.AplicacionWeb.Models.ViewModels;
using SistemaVentas.AplicacionWeb.Utilidades.Response;
using SistemaVentas.BLL.Interfaces;
using SistemaVentas.Entity;

namespace SistemaVentas.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        
        private readonly IUsuarioS _usuarios;
        private readonly IRoles _roleservicio;
        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioS usuarios, IRoles roleservicio, IMapper mapper)
        {
            _usuarios= usuarios;
            _roleservicio= roleservicio;
            _mapper=mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<VMRol> Vmlistaroles= _mapper.Map<List<VMRol>>(await _roleservicio.lista());
            return StatusCode(StatusCodes.Status200OK,Vmlistaroles);
        }
        [HttpGet]
        public async Task<IActionResult> ListaUsuarios()
        {
            List<VMUsuario> Vmlistausuario = _mapper.Map<List<VMUsuario>>(await _usuarios.ListaUsuario());
            return StatusCode(StatusCodes.Status200OK, new {data= Vmlistausuario});
        }
        [HttpPost]
        public async Task<IActionResult> CrearUsuarios([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();
            try
            {
                VMUsuario vmusuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);
                string nombrefoto = "";
                Stream fotostream = null;
                if (foto!=null)
                {
                    string nombrecodigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombrefoto = string.Concat(nombrecodigo,extension);
                    fotostream = foto.OpenReadStream();
                }
                string urlplantillacorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";
                Usuario usuariocreado = await _usuarios.CrearUsuario(_mapper.Map<Usuario>(vmusuario), fotostream, nombrefoto, urlplantillacorreo);
                vmusuario = _mapper.Map<VMUsuario>(usuariocreado);
                response.Estado = true;
                response.Objeto = vmusuario;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpPut]
        public async Task<IActionResult> EditarUsuarios([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();
            try
            {
                VMUsuario vmusuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);
                string nombrefoto = "";
                Stream fotostream = null;
                if (foto != null)
                {
                    string nombrecodigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombrefoto = string.Concat(nombrecodigo, extension);
                    fotostream = foto.OpenReadStream();
                }
                Usuario usuarioeditado = await _usuarios.EditarUsuario(_mapper.Map<Usuario>(vmusuario), fotostream, nombrefoto);
                vmusuario = _mapper.Map<VMUsuario>(usuarioeditado);
                response.Estado = true;
                response.Objeto = vmusuario;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpDelete]
        public async Task<IActionResult> EliminarUsuarios(int id)
        {
            GenericResponse<string> response = new GenericResponse<string>();
            try
            {
                response.Estado = await _usuarios.EliminarUsuario(id);
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
