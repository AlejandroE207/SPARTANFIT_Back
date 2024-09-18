using Microsoft.AspNetCore.Mvc;
using SPARTANFIT.Dto;
using SPARTANFIT.Services;

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
    }
}
