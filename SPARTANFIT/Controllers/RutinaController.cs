using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;

namespace SPARTANFIT.Controllers
{
    public class RutinaController : Controller
    {
        private readonly EntrenadorService _entrenadorService;

        public RutinaController(EntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }

        [HttpPost("RegistrarRutina")]
        public async Task<IActionResult> RegistrarRutina([FromForm] int[] selectedCheckboxIds, [FromForm] int[] listadoSeries, [FromForm] int[] listadoRepeticiones, RutinaDto rutina)
        {
            List<EjercicioDto> ejerciciosRutina = new List<EjercicioDto>();
            if (selectedCheckboxIds != null)
            {

                for (int i = 0; i < selectedCheckboxIds.Length; i++)
                {
                    int checkboxId = selectedCheckboxIds[i];
                    int series = listadoSeries[i];
                    int repeticiones = listadoRepeticiones[i];

                    EjercicioDto ejercicio = new EjercicioDto
                    {
                        id_ejercicio = checkboxId,
                        num_series = series,
                        repeticiones = repeticiones
                    };

                    ejerciciosRutina.Add(ejercicio);
                }
            }
            int resultado = await _entrenadorService.RegistrarRutina(rutina, ejerciciosRutina);
            if (resultado == 0)
            {
                return NotFound("No fue posible crear la rutina de entrenamiento");
            }
            else
            {
                return Ok(new { mensaje = "Creacion de la rutina exitosa" });
            }
        }
        
        [HttpGet("MostrarRutinas")]
        public async Task<IActionResult> MostrarRutinas()
        {
            List<RutinaDto>listRutinas = new List<RutinaDto>();
            listRutinas = await _entrenadorService.MostrarRutinas();
            if(listRutinas.Count == 0)
            {
                return NotFound("No hay rutinas registradas por el momento");
            }
            return Ok(listRutinas);
        }

        [HttpPost("EliminarRutina")]
        public async Task<IActionResult> EliminarRutina([FromQuery]int id_rutina)
        {
            int resultado = 0;
            resultado = await _entrenadorService.EliminarRutina(id_rutina);
            if(resultado == 0)
            {
                return NotFound("No se pudo eliminar la rutina de entrenamiento");
            }
            return Ok(new { mensaje = "Eliminacion exitosa" });
        }

        [HttpGet("DetallesRutina")]
        public async Task<IActionResult> DetallesRutina([FromQuery] int id_rutina)
        {
            try
            {
                (RutinaDto rutina, List<EjercicioDto> listEjercicios) = await _entrenadorService.DetallesRutina(id_rutina);
                if(rutina == null || listEjercicios == null || !listEjercicios.Any())
                {
                    return NotFound("No se encontro los detalles de la rutina");
                }
                var response = new
                {
                    rutina = rutina,
                    ejercicios = listEjercicios
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Se produjo un error al procesar la solicitud: {ex.Message}");
            }
        }
    }
}
