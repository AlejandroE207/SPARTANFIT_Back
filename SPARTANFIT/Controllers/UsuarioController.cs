using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;

using System.Threading.Tasks;

namespace SPARTANFIT_BACKEND.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioDto usuario)
        {
           

            try
            {
                var correo = usuario?.persona?.correo;
               

                if (await _usuarioService.BuscarPersona(correo))
                {
                    return BadRequest( "Cuenta ya registrada." );
                }
                await _usuarioService.RegistroUsuarioAsync(usuario);
                return Ok(new { mensaje = "Usuario registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al registrar el usuario." + ex.Message );
            }
        }

        [HttpGet("Traer-Datos-Usuario")]
        public async Task<IActionResult> ObtenerDatosPersona([FromQuery] int id_usuario)
        {
            var usuario = await _usuarioService.EnviarDatosUsu(id_usuario);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrada.");
            }


            return Ok(new
            {

                nombres = usuario.persona.nombres,
                apellidos = usuario.persona.apellidos,
                fecha_nacimiento = usuario.persona.fecha_nacimiento,
                genero = usuario.persona.genero,
                estatura = usuario.estatura,
                peso = usuario.peso,
                id_objetivo = usuario.id_objetivo,
                nivel_entrenamiento = usuario.id_nivel_entrenamiento,
                rehabilitacion = usuario.rehabilitacion,
            });
        }

    }
}
