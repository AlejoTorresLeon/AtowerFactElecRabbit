using System.Threading.Tasks;
using AtowerEnvioNubex.Aplicacion.Dtos;
using AtowerEnvioNubex.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AtowerEnvioNubex.Presentacion.EnviarNubexDian
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviarDianController : ControllerBase
    {
        private readonly IEnvioFacturaNubexDian _envioFacturaNubexDian;

        public EnviarDianController(IEnvioFacturaNubexDian envioFacturaNubexDian)
        {
            _envioFacturaNubexDian = envioFacturaNubexDian;
        }

        [HttpPost("enviarFactura")]
        public async Task<IActionResult> EnviarFactura([FromBody] FacturaDian factura,int? idCompani)
        {
            if (factura == null || idCompani == null)
            {
                return BadRequest("La factura y el Id de la compañía son requeridos.");
            }

            var response = await _envioFacturaNubexDian.EnviarFacturaNubexDian(factura, idCompani);

            if (response == null)
            {
                return NotFound("No se encontró un token válido para la compañía proporcionada.");
            }

            return Ok(response);
        }
    }
}
