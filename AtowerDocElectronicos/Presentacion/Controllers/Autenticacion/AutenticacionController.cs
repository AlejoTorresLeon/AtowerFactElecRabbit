using AtowerDocElectronico.Aplicacion.Dtos.Autenticacion;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Aplicacion.Services.Autenticacion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtowerDocElectronico.Presentacion.Controllers.Autenticacion
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly ILogin _autenticacion;

        public AutenticacionController(ILogin autenticacion)
        {
            _autenticacion = autenticacion;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTOs usuarioDto)
        {
           var token = await _autenticacion.GetUsuario(usuarioDto);
            
            return Ok(token);
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar(CrearUsuario usuarionew)
        {
            var user = await _autenticacion.CreateUser(usuarionew);

            return Ok(user);
        }

        [Authorize]
        [HttpPost("Prueba")]
        public async Task<IActionResult> PruebaAsync()
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken == null)
            {
                return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
            }
          

            // Llamar al método GetTokenIdUsuario con el token obtenido
            var tokenResultado = _autenticacion.GetTokenIdUsuario(accessToken);

            // Verificar si se obtuvo un token válido
            if (!string.IsNullOrEmpty(tokenResultado))
            {
                // Si se obtuvo un token válido, devolverlo en la respuesta
                return Ok(tokenResultado);
            }
            else
            {
                // Si no se pudo obtener un token válido, devolver un mensaje de error
                return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
            }
        }

    }
}
