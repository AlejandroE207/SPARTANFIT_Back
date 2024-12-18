﻿using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPARTANFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjercicioController : ControllerBase
    {
        private readonly EntrenadorService _entrenadorService;
        public EjercicioController(EntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }

        [HttpPost("RegistrarEjercicio")]
        public async Task<IActionResult> RegistrarEjercicio([FromBody]EjercicioDto ejercicio)
        {
            int resultado = 0;
            resultado = await _entrenadorService.RegistrarEjercicio(ejercicio);
            switch (resultado)
            {
                case -1: return NotFound("Problema en registro de ejercicios");
                    break;
                case 0: return BadRequest("Ejercicio ya existente!");
                    break;
                case 1: return Ok(new {mensaje = "Ejercicio registrado exitosamente"});
                    break;
                default: return BadRequest("Paso algo!");
                    break;
            }
        }

        [HttpPost("EliminarEjercicio")]
        public async Task<IActionResult> EliminarEjercicio([FromQuery]int id_ejercicio)
        {
            int resultado = 0;
            resultado = await _entrenadorService.EliminarEjercicio(id_ejercicio);
            if(resultado == 0)
            {
                return NotFound("No se pudo eliminar el ejercicio");
            }
            else
            {
                return Ok(new { mensaje = "Eliminacion de ejercicio exitosa" });
            }
        }

        [HttpPost("ActualizarEjercicio")]
        public async Task<IActionResult> ActualizarEjercicio([FromForm] int id_ejercicio, [FromForm] string nombre_ejercicio, [FromForm] int id_grupo_muscular, [FromForm] string apoyo_visual)
        {
            try
            {
                EjercicioDto ejercicio = new EjercicioDto
                {
                    id_ejercicio = id_ejercicio,
                    nombre_ejercicio = nombre_ejercicio,
                    id_grupo_muscular = id_grupo_muscular,
                    apoyo_visual = apoyo_visual,
                };

                int resultado = await _entrenadorService.ActualizarEjercicio(ejercicio);

                if (resultado == 0)
                {
                    return NotFound("Error al actualizar el ejercicio");
                }

                return Ok(new { mensaje = "Actualización de ejercicio exitosa" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet("ListEjercicios")]
        public async Task<IActionResult> MostrarEjercicios()
        {
            List<EjercicioDto> listEjercicios = new List<EjercicioDto>();
            listEjercicios = await _entrenadorService.MostrarEjercicios();
            if(listEjercicios.Count == 0)
            {
                return NotFound("No hay ejercicios registrados por el momento");
            }
            return Ok(listEjercicios);
        }
    }
}
