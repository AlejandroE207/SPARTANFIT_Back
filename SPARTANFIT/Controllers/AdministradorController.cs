using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;
using SPARTANFIT.Utilitys;
using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPARTANFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdministradorController : ControllerBase
    {
        private readonly AdministradorService _administradorService;
        private readonly UsuarioService _usuarioService;
        private readonly CorreoUtility _correoUtility;
        private readonly PersonaService _personaService;

        public AdministradorController(AdministradorService administradorService, UsuarioService usuarioService, CorreoUtility correoUtility, PersonaService personaService)
        {
            _administradorService = administradorService;
            _usuarioService = usuarioService;
            _correoUtility = correoUtility;
            _personaService = personaService;
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

            if(await _usuarioService.BuscarPersona(entrenador.correo))
            {
                return BadRequest("Entrenador ya existente");
            }
            else
            {
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

        [HttpPost("CorreoReporteUsuarios")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>CorreoReporteUsuarios([FromQuery] int id_usuario)
        {
            PersonaDto persona = new PersonaDto();
            persona = await _personaService.SeleccionarPersona(id_usuario);
            string pdfFilePath = await _administradorService.CrearPdfUsuarios();

            if (string.IsNullOrEmpty(pdfFilePath) || !System.IO.File.Exists(pdfFilePath))
            {
                return StatusCode(500, "No se pudo generar el PDF de usuarios.");
            }

            string mensajeCorreo = "Adjunto se encuentra el reporte en PDF de los usuarios registrados.";
            try
            {
                _correoUtility.EnviarCorreoConAdjunto(persona.correo, "Reporte de Usuarios Registrados", mensajeCorreo, pdfFilePath);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo: {ex.Message}");
            }

            System.IO.File.Delete(pdfFilePath);

            return Ok(new { mensaje = "Reporte enviado correctamente." });
        }

        
        [HttpPost("CorreoReporteEntrenadores")]
        public async Task<IActionResult> CorreoReporteEntrenadores([FromQuery] int id_usuario)
        {

            PersonaDto persona = new PersonaDto();
            persona = await _personaService.SeleccionarPersona(id_usuario);
            string pdfFilePath = await _administradorService.CrearPdfEntrenadores();

            if (string.IsNullOrEmpty(pdfFilePath) || !System.IO.File.Exists(pdfFilePath))
            {
                return StatusCode(500, "No se pudo generar el PDF de entrenadores.");
            }

            string mensajeCorreo = "Adjunto se encuentra el reporte en PDF de los entrenadores registrados.";
            try
            {
                byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfFilePath);

                _correoUtility.EnviarCorreoConAdjunto(persona.correo, "Reporte de Entrenadores Registrados", mensajeCorreo, pdfFilePath);

                await Task.Delay(500); 
                System.IO.File.Delete(pdfFilePath);
            }
            catch (IOException ioEx)
            {
                return StatusCode(500, $"Error al acceder o eliminar el archivo: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo: {ex.Message}");
            }

            return Ok(new { mensaje = "Reporte enviado correctamente." });
        }


    }
}
