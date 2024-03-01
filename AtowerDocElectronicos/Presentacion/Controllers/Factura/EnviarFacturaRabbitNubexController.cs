using AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Dtos.Factura;
using AtowerDocElectronico.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtowerDocElectronico.Presentacion.Controllers.Factura
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnviarFacturaRabbitNubexController : ControllerBase
    {
        private readonly IEnviarFacturaRabbitNubex _enviarRabbit;
        private readonly ILogin _autenticacion;

        public EnviarFacturaRabbitNubexController(IEnviarFacturaRabbitNubex enviarRabbit, ILogin autentication)
        {
            _enviarRabbit = enviarRabbit;
            _autenticacion = autentication;
        }

        [HttpPost("EnviarFacturaCola")]
        public async Task<IActionResult> EnviarFacturaCola([FromBody] FacturaAtowerDto_Base64pdf fasturaAtower)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
            }
            
            var factura = await _enviarRabbit.EnviarFacturaRabbit(fasturaAtower.FacturaAtowerDTO, idUsuario ?? 0, fasturaAtower.Base64Pdf);

            if (factura.Success != true)
            {
                return BadRequest(factura);
            }

            return Ok(factura);
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
