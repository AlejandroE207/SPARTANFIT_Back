using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;
using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPARTANFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly AdministradorService _administradorService;

        public AdministradorController(AdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        [HttpGet("ListUsuarios")]
        public async Task<IActionResult> Mostrar_Usuarios()
        {
            List<UsuarioDto> listUsuarios = new List<UsuarioDto>();
            listUsuarios = await _administradorService.Mostrar_Usuario();

            if (listUsuarios.Count == 0)
            {
                return NotFound("No hay usuarios registrados");
            }

            return Ok(listUsuarios);
        }

        [HttpGet("ListEntrenadores")]
        public async Task<IActionResult> Mostrar_Entrenadores()
        {
            List<PersonaDto> listEntrenadores = new List<PersonaDto>();
            listEntrenadores = await _administradorService.Mostrar_Entrenadores();

            if (listEntrenadores.Count == 0)
            {
                return NotFound("No hay entrenadores registrados");
            }

            return Ok(listEntrenadores);

        }

        [HttpPost("RegistrarEntrenador")]
        public async Task<IActionResult> Registrar_entrenador([FromBody]PersonaDto entrenador)
        {
            int resultado = 0;
            resultado = await _administradorService.Registrar_Entrenadores(entrenador);
            if(resultado == 0)
            {
                return NotFound("Problema en registro del entrenador");
            }
            else
            {
                return Ok(new {mensaje = "Entrenador registrado exitosamente"});

            }
        }

        [HttpPost("ActualizarEntrenador")]
        public async Task<IActionResult> Actualizar_entrenador([FromForm]int id_usuario, [FromForm]string nombres, [FromForm]string apellidos, [FromForm]string correo)
        {
            int resultado = 0;
            PersonaDto entrenador = new PersonaDto();
            entrenador.id_usuario = id_usuario;
            entrenador.nombres = nombres;
            entrenador.apellidos = apellidos;
            entrenador.correo= correo;
            resultado = await _administradorService.Actualizar_Entrenador(entrenador);
            if(resultado == 0)
            {
                return NotFound("Problema en actualizar entrenador");
            }
            else
            {
                return Ok(new {mensaje="Datos actualizados exitosamente!"});
            }
        }

        [HttpPost("EliminarEntrenador")]
        public async Task<IActionResult> Eliminar_Entrenador([FromQuery]int id)
        {
            int resultado = 0;
            resultado = await _administradorService.Eliminar_Entrenador(id);
            if (resultado == 0)
            {
                return NotFound("Error al eliminar entrenador!");
            }
            else
            {
                return Ok(new { mensaje = "Entrenador Eliminado exitosamente!!" });
            }
        }

        [HttpGet("ReporteUsuarios")]
        public async Task<IActionResult> Generar_Reporte_Usuarios()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Usuarios.pdf");

            await _administradorService.CrearPdfUsuarios();

            var pdfBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);

            System.IO.File.Delete(tempFilePath);

            return File(pdfBytes, "application/pdf", "Lista_Usuarios.pdf");
        }

        [HttpGet("ReporteEntrenadores")]
        public async Task<IActionResult> Generar_Reporte_Entrenadores()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Entrenadores.pdf");

            await _administradorService.CrearPdfEntrenadores();

            var pdfBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);

            System.IO.File.Delete(tempFilePath);

            return File(pdfBytes, "application/pdf", "Lista_Entrenadores.pdf");
        }

    }
}
