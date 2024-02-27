using AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Aplicacion.Services.ConfiguracionAtowerNubex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtowerDocElectronico.Presentacion.Controllers.ConfiguracionAtowerNubex
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NubexAtowerController : ControllerBase
    {
        private readonly ICompañia _companiaService;
        private readonly ILogin _autenticacion;

        public NubexAtowerController(ICompañia companiaService, ILogin autenticacion )
        {
            _companiaService = companiaService;
            _autenticacion = autenticacion;
        }

        [HttpPost("CrearCompañia")]
        public async Task<IActionResult> CrearCompania([FromBody] CompañiaPostDto compani)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
            }

            var result = await _companiaService.CrearCompañia(compani, idUsuario.Value);

            if (result?.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        private int? ObtenerIdUsuario()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            if (accessToken == null)
            {
                return null;
            }

            var validIdUsuario = _autenticacion.GetTokenIdUsuario(accessToken);
            return validIdUsuario != null ? int.Parse(validIdUsuario) : (int?)null;
        }
    }
}
