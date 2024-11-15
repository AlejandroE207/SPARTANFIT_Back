using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Services;
using SPARTANFIT.Utilitys;


namespace SPARTANFIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly PersonaService _personaService;
        private readonly TokenUtility _tokenUtility;

        public PersonaController(PersonaService personaService, TokenUtility tokenUtility)
        {
            _personaService = personaService;
            _tokenUtility = tokenUtility;
        }

        [HttpPost("IniciarSesion")]
        
        public async Task<IActionResult> IniciarSesion([FromForm] string correo, [FromForm] string contrasena)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
            {
                return BadRequest("Correo y contraseña son requeridos.");
            }

            try
            {
                bool resultado = await _personaService.IniciarSesion(correo, contrasena);

                if (resultado)
                {
                    var token = _tokenUtility.GenerarToken(correo);
                    Console.WriteLine("Token: "+ token);
                    return Ok(new
                    {
                        Mensaje = "Inicio de sesión exitoso",
                        Token = token
                    });
                }
                else
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al iniciar sesión: " + ex.Message);
            }
        }
        [HttpPost("EnviarCodigo")]
        public async Task<IActionResult> EnviarCodigo([FromForm] string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                return BadRequest("El correo es requerido.");
            }

            try
            {
                var persona = await _personaService.enviarCodigo(correo);

                if (persona != null && persona.id_usuario > 0)
                {
                    return Ok("Código de recuperación enviado correctamente.");
                }
                else
                {
                    return NotFound("Usuario no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al enviar el código: " + ex.Message);
            }
        }
        [HttpPost("RestablecerContrasena")]
        public async Task<IActionResult> RestablecerContrasena([FromForm] string codigo, [FromForm] string contrasena)
        {
            if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(contrasena))
            {
                return BadRequest("El código y la contraseña son requeridos.");
            }

            try
            {
              
                int filasAfectadas = await _personaService.ActualizarContrasena(contrasena, codigo);

                if (filasAfectadas > 0)
                {
                    return Ok("Contraseña restablecida con éxito.");
                }
                else
                {
                    return BadRequest("No se pudo restablecer la contraseña. Verifica el código.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al restablecer la contraseña: " + ex.Message);
            }
        }
        [HttpGet("Traer-Datos-Personas")]
        public async Task<IActionResult> ObtenerDatosPersona([FromQuery] string correo)
        {
            var persona = await _personaService.EnviarPersonas(correo);

            if (persona == null)
            {
                return NotFound("Persona no encontrada.");
            }

        
            return Ok(new
            {
                id_usuario = persona.id_usuario,
                id_rol = persona.id_rol,
               
            });
        }


    }
}
