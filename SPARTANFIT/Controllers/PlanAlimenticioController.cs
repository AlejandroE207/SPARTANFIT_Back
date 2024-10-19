using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Services;
using SPARTANFIT.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SPARTANFIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanAlimenticioController : ControllerBase
    {
        private readonly EntrenadorService _entrenadorService;

        public PlanAlimenticioController(EntrenadorService entrenadorService)
        {
            _entrenadorService = entrenadorService;
        }

        [HttpPost("RegistrarPlanAlimenticio")]
        public async Task<IActionResult> RegistrarPlanAlimenticio([FromForm] PlanAlimenticioDto planAlimenticio, [FromForm] int[] selectedCheckboxIds)
        {
            List<int> idAlimentos = new List<int>();
            for(int i = 0;i< selectedCheckboxIds.Length; i++)
            {
                idAlimentos.Add(i);
            }

            int resultado = await _entrenadorService.RegistrarPlanAlimenticio(planAlimenticio, idAlimentos);

            if(resultado != 0) 
            {
                return NotFound("No fue posible crear el plan alimenticio");
            }
            else
            {
                return Ok(new { mensaje = "Creacion de plan alimenticio exitoso"});
            }
        }

        [HttpGet("MostrarPlanesAlimenticios")]
        public async Task<IActionResult> MostrarPlanAlimenticios()
        {

            List<PlanAlimenticioDto> listPlanes = new List<PlanAlimenticioDto>();
            listPlanes = await _entrenadorService.MostrarPlanes();
            if(listPlanes.Count == 0)
            {
                return NotFound("No hay planes alimenticios registrados por el momento");
            }
            return Ok(listPlanes);
        }

        [HttpPost("EliminarPlan")]
        public async Task<IActionResult> EliminarPlan([FromQuery]int id_plan_alimenticio)
        {
            int resultado = 0;
            resultado = await _entrenadorService.EliminarPlan(id_plan_alimenticio);
            if(resultado == 0)
            {
                return NotFound("No es posible eliminar la rutina de entrenamiento");
            }
            return Ok(new { mensaje = "Eliminacion exitosa" });
        }
    }
}
