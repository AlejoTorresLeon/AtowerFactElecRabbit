
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
        public async Task<IActionResult> EnviarCorreo(string destinatario, string asunto, string servidorSmtp, int PuertoSmtp, string usuarioEmail, string passwordEmail,string base64)
        {
            await _correoService.EnviarCorreoAsync(destinatario, asunto,  servidorSmtp, PuertoSmtp, usuarioEmail, passwordEmail, base64);
            return Ok("Correo enviado exitosamente.");
        }
    }
}
