
using AtowerEnvioCorreos.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace AtowerEnvioCorreos.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvioCorreoController : ControllerBase
    {
        private readonly ICorreoService _correoService;

        public EnvioCorreoController(ICorreoService correoService)
        {
            _correoService = correoService;
        }

        [HttpPost]
        public async Task<IActionResult> EnviarCorreo(string destinatario)
        {
            await _correoService.EnviarCorreoAsync(destinatario);
            return Ok("Correo enviado exitosamente.");
        }
    }
}
