﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("ActualizarObjetivo")]
        public async Task<IActionResult> ActualizarObjetivo([FromForm] int id_usuario,[FromForm]int rehabilitacion, [FromForm]int id_nivel_entrenamiento, [FromForm]int id_objetivo)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            usuario.persona.id_usuario = id_usuario;
            usuario.rehabilitacion = rehabilitacion;
            usuario.id_objetivo = id_objetivo;
            usuario.id_nivel_entrenamiento = id_nivel_entrenamiento;

            if(usuario.rehabilitacion == 1)
            {
                usuario.id_objetivo = 0;
                usuario.id_nivel_entrenamiento = 0;
            }
            int resultado = await _usuarioService.ActualizarObjetivo(usuario);
            if(resultado == 0)
            {
                return NotFound("No fue posible actualizar los datos del objetivo");
            }
            else
            {
                return Ok(new { mensaje = "Actualizacion exitosa" });
            }
        }

        [HttpPost("EliminarUsuario")]
        public async Task<IActionResult> EliminarCuenta([FromQuery] int id_usuario)
        {
            int resultado = 0;
            resultado = await _usuarioService.EliminarUsuario(id_usuario);
            if (resultado == 0)
            {
                return NotFound("No fue posible eliminar la cuenta");
            }
            else
            {
                return Ok(new { mensaje = "Eliminacion exitosa" });
            }

        }

        [HttpPost("ActualizarDatos")]
        public async Task<IActionResult> ActualizarDatos([FromForm]int id_usuario, [FromForm] double peso, [FromForm] double estatura)
        {
            int resultado = 0;
            UsuarioDto usuario = new UsuarioDto();
            usuario.persona = new PersonaDto();
            usuario.persona.id_usuario = id_usuario;
            usuario.peso = peso;
            usuario.estatura = estatura;
            resultado = await _usuarioService.ActualizarDatos(usuario);
            if(resultado == 0)
            {
                return NotFound("No fue posible actualizar datos");
            }
            else
            {
                return Ok(new { mensaje = "Actualizacion de datos exitosa" });
            }
        }

        [HttpGet("MostrarRutinaDia")]
        public async Task<IActionResult> MostrarRutinaDia([FromQuery] int id_usuario)
        {
            try
            {
                UsuarioDto usuario = await _usuarioService.SeleccionarUsuarioAsync(id_usuario);
                (RutinaDto rutinaDia, List<EjercicioDto> listEjerciciosDia) = await _usuarioService.MostrarRutinaDia(usuario);
                if (rutinaDia == null || listEjerciciosDia == null || !listEjerciciosDia.Any())
                {
                    return NotFound("No se encontró la rutina o los ejercicios para el día.");
                }
                var response = new
                {
                    Rutina = rutinaDia,
                    Ejercicios = listEjerciciosDia
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Se produjo un error al procesar la solicitud: {ex.Message}");
            }
        }

        [HttpGet("MostrarPlanAlimenticioDia")]
        public async Task<IActionResult> MostrarPlanAlimenticioDia([FromQuery] int id_usuario)
        {
            try
            {
                UsuarioDto usuario = await _usuarioService.SeleccionarUsuarioAsync(id_usuario);
                (PlanAlimenticioDto planAlimenticioDia, List<AlimentoDto> listAlimenticioDia) = await _usuarioService.MostrarPlanALimenticioDia(usuario);
                if(planAlimenticioDia == null || listAlimenticioDia == null || !listAlimenticioDia.Any())
                {

                    return NotFound("No se encontro el plan alimenticio o los alimentos para el dia");
                }
                var response = new
                {
                    PlanAlimenticio = planAlimenticioDia,
                    Alimentos = listAlimenticioDia
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Se produjo un error al procesar la solicitud: {ex.Message}");
            }
        }



    }
}
