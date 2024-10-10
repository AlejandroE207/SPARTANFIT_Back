using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPARTANFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlimentoController : ControllerBase
    {
        private readonly EntrenadorService _entrenadorService;
        public AlimentoController(EntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }


        [HttpGet("ListAlimentos")]
        public async Task<IActionResult> MostrarAlimentos()
        {
            List<AlimentoDto>listAlimentos = new List<AlimentoDto>();
            listAlimentos = await _entrenadorService.MostrarAlimentos();
            if(listAlimentos.Count == 0)
            {
                return NotFound("No hay alimentos registrados por el momento");
            }
            return Ok(listAlimentos);
        }

        [HttpPost("RegistrarAlimento")]
        public async Task<IActionResult> RegistrarAlimento([FromBody]AlimentoDto alimento)
        {
            int resultado = 0;
            resultado = await _entrenadorService.RegistrarAlimento(alimento);
            switch (resultado)
            {
                case -1:
                    return NotFound("Problema en registro de ejercicios");
                    break;
                case 0:
                    return BadRequest("Ejercicio ya existente!");
                    break;
                case 1:
                    return Ok(new { mensaje = "Ejercicio registrado exitosamente" });
                    break;
                default:
                    return BadRequest("Paso algo!");
                    break;
            }
        }

        [HttpPost("ActualizarAlimento")]
        public async Task<IActionResult> ActualizarAlimento([FromForm]int id_alimento, [FromForm]int id_categoria_alimento, [FromForm] string nombre, [FromForm] double calorias_x_gramo,
            [FromForm] double grasa, [FromForm] double carbohidrato, [FromForm] double proteina, [FromForm] double fibra)
        {
            int resultado = 0;
            AlimentoDto alimento = new AlimentoDto();
            alimento.id_alimento = id_alimento;
            alimento.id_categoria_alimento = id_categoria_alimento;
            alimento.nombre = nombre;
            alimento.calorias_x_gramo = calorias_x_gramo;
            alimento.grasa = grasa;
            alimento.carbohidrato = carbohidrato;
            alimento.proteina = proteina;
            alimento.fibra = fibra;
            resultado = await _entrenadorService.ActualizarAlimento(alimento);
            if(resultado == 0)
            {
                return NotFound("Error al actualizar el ejercicio");
            }
            else
            {
                return Ok(new { mensaje = "Actualizacion de ejercicio exitoso" });
            }
        }
        [HttpPost("EliminarAlimento")]
        public async Task<IActionResult> ElimianarAlimento([FromQuery]int id_alimento)
        {
            int resultado = 0;
            resultado = await _entrenadorService.EliminarAlimento(id_alimento);
            if(resultado == 0)
            {
                return NotFound("No se pudo eliminar el alimento");
            }
            else
            {
                return Ok(new { mensaje = "Eliminacion de alimento exitosa!" });
            }
        }
    }
}
