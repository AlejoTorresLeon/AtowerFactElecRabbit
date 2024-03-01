using AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtowerDocElectronico.Presentacion.Controllers.ConfiguracionAtowerNubex
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfiguracionCompañiaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEnviarHttp _enviarHttp;
        private readonly ILogin _autenticacion;


        public ConfiguracionCompañiaController(IConfiguration configuration, IEnviarHttp enviarHttp, ILogin autenticacion)
        {
            _configuration = configuration;
            _enviarHttp = enviarHttp;
            _autenticacion = autenticacion;
        }

        [HttpPut("RegistrarSoftware")]
        public async Task<IActionResult> RegistrarCompany([FromBody] RegistrarSoftwareDTO registrarConpany)
        {
            try
            {
                var idUsuario = ObtenerIdUsuario();
                if (idUsuario == null)
                {
                    return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
                }

                var idRol = ObtenerIdRol();

                if (idRol != 4) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return Forbid(); // Regresa un código de estado Forbidden
                }

                var tokenBearer = ObtenerTokenNubex();

                if (tokenBearer == null) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return BadRequest("No se pudo obtener el token Nubex para el usuario proporcionado.");
                }


                

                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/software";
         

                var transformedData = TrandormacionRegistrarCompany(registrarConpany);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Put, content, tokenBearer);
                return HandleResponseGenerico(response);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada (por ejemplo, registrarla)
                //return StatusCode(500, "Error interno del servidor");
                return StatusCode(500, new { message = "Error interno del servidor" });


            }

        }

        [HttpPut("RegistrarCertificado")]
        public async Task<IActionResult> RegistrarCertificado([FromBody] RegistrarCertificadoDTo registrarCertificado)
        {
            try
            {

                var idUsuario = ObtenerIdUsuario();
                if (idUsuario == null)
                {
                    return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
                }

                var idRol = ObtenerIdRol();

                if (idRol != 4) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return Forbid(); // Regresa un código de estado Forbidden
                }

                var tokenBearer = ObtenerTokenNubex();

                if (tokenBearer == null) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return BadRequest("No se pudo obtener el token Nubex para el usuario proporcionado.");
                }

                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/certificate";


                

                var transformedData = TrandormacionRegistrarCertificado(registrarCertificado);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Put, content, tokenBearer);
                return HandleResponseGenerico(response);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada (por ejemplo, registrarla)
                //return StatusCode(500, "Error interno del servidor");
                return StatusCode(500, new { message = "Error interno del servidor" });


            }
        }

        [HttpPut("RegistrarResolucion")]
        public async Task<IActionResult> RegistrarResolucion([FromBody] RegistrarResolucionDTo registrarResolucion)
        {
            try
            {                
                var idUsuario = ObtenerIdUsuario();
                if (idUsuario == null)
                {
                    return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
                }

                var idRol = ObtenerIdRol();

                if (idRol != 4) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return Forbid(); // Regresa un código de estado Forbidden
                }

                var tokenBearer = ObtenerTokenNubex();

                if (tokenBearer == null) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return BadRequest("No se pudo obtener el token Nubex para el usuario proporcionado.");
                }


                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/resolution";

                var transformedData = TrandormacionRegistrarResolucion(registrarResolucion);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Put, content, tokenBearer);
                return HandleResponseGenerico(response);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada (por ejemplo, registrarla)
                //return StatusCode(500, "Error interno del servidor");
                return StatusCode(500, new { message = "Error interno del servidor" });


            }
        }

        [HttpPut("RegistrarResolucionNotaDebito")]
        public async Task<IActionResult> RegistrarResolucionNotaDebito([FromBody] RegistrarResolucionNotasDTo registrarResolucion)
        {
            try
            {
                var idUsuario = ObtenerIdUsuario();
                if (idUsuario == null)
                {
                    return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
                }

                var idRol = ObtenerIdRol();

                if (idRol != 4) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return Forbid(); // Regresa un código de estado Forbidden
                }

                var tokenBearer = ObtenerTokenNubex();

                if (tokenBearer == null) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return BadRequest("No se pudo obtener el token Nubex para el usuario proporcionado.");
                }

                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/resolution";

                var transformedData = TrandormacionRegistrarNotaDebito(registrarResolucion);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Put, content, tokenBearer);
                return HandleResponseGenerico(response);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada (por ejemplo, registrarla)
                //return StatusCode(500, "Error interno del servidor");
                return StatusCode(500, new { message = "Error interno del servidor" });


            }
        }


        [HttpPut("RegistrarResolucionNotaCredito")]
        public async Task<IActionResult> RegistrarResolucionNotaCredito([FromBody] RegistrarResolucionNotasDTo registrarResolucion)
        {
            try
            {
                var idUsuario = ObtenerIdUsuario();
                if (idUsuario == null)
                {
                    return BadRequest("No se pudo obtener el token para el usuario proporcionado.");
                }

                var idRol = ObtenerIdRol();

                if (idRol != 4) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return Forbid(); // Regresa un código de estado Forbidden
                }

                var tokenBearer = ObtenerTokenNubex();

                if (tokenBearer == null) // Reemplaza esto con tu lógica real de verificación de permisos
                {
                    return BadRequest("No se pudo obtener el token Nubex para el usuario proporcionado.");
                }

                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/resolution";

                var transformedData = TrandormacionRegistrarNotaCredito(registrarResolucion);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Put, content, tokenBearer);
                return HandleResponseGenerico(response);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada (por ejemplo, registrarla)
                //return StatusCode(500, "Error interno del servidor");
                return StatusCode(500, new { message = "Error interno del servidor" });


            }
        }
        private object TrandormacionRegistrarNotaCredito(RegistrarResolucionNotasDTo registrarResolucion)
        {
            return new
            {
                type_document_id = 4,
                prefix = registrarResolucion.Prefijo,
                from = registrarResolucion.RangoDesde,
                to = registrarResolucion.RangoHasta
            };
        }

        private object TrandormacionRegistrarNotaDebito(RegistrarResolucionNotasDTo registrarResolucion)
        {
            return new
            {
                type_document_id = 5,
                prefix = registrarResolucion.Prefijo,
                from = registrarResolucion.RangoDesde,
                to = registrarResolucion.RangoHasta
            };
        }

        private object TrandormacionRegistrarResolucion(RegistrarResolucionDTo registrarResolucion)
        {
            return new
            {
                type_document_id = 1,
                prefix = registrarResolucion.Prefijo,
                resolution = registrarResolucion.NumeroResolucion,
                resolution_date = registrarResolucion.FechaResolucion,
                technical_key = registrarResolucion.ClaveTecnica,
                from = registrarResolucion.RangoDesde,
                to = registrarResolucion.RangoHasta,
                generated_to_date = 0,
                date_from = registrarResolucion.FechaInicio,
                date_to = registrarResolucion.FechaFin
            };
        }

        private object TrandormacionRegistrarCertificado(RegistrarCertificadoDTo registrarCertificadoDTo)
        {
            return new
            {
                certificate = registrarCertificadoDTo.CertificadoBase64,
                password = registrarCertificadoDTo.password
            };
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

        private int? ObtenerIdRol()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            if (accessToken == null)
            {
                return null;
            }

            var validIdRol = _autenticacion.GetTokenIdRol(accessToken);
            return validIdRol != null ? int.Parse(validIdRol) : (int?)null;
        }

        private string? ObtenerTokenNubex()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            if (accessToken == null)
            {
                return null;
            }

            var validIdRol = _autenticacion.GetTokenNubex(accessToken);
            return validIdRol != null ? (validIdRol) : (string?)null;
        }
        private object TrandormacionRegistrarCompany(RegistrarSoftwareDTO registrarConpany)
        {
            return new
            {
                id = registrarConpany.Id,
                pin = registrarConpany.Pin
            };
        }

        private IActionResult HandleResponseGenerico(HttpResponseMessage response)
        {
            var responseBody = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                return Ok(BuildSuccessResponseGenerico(responseBody));
            }

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                return UnprocessableEntity(BuildErrorResponse(responseBody));
            }

            return BadRequest(responseBody);
        }


        private object BuildSuccessResponseGenerico(string responseBody)
        {
            var responseObject = JsonConvert.DeserializeObject<RegistrarCompanyResponseDtos>(responseBody);

            return new
            {
                message = responseObject.message

            };
        }

        private object BuildErrorResponse(string responseBody)
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(responseBody);
                return new
                {
                    error = "Validation error",
                    details = errorResponse.Errors // o alguna otra propiedad que contenga los detalles del error
                };
            }
            catch (JsonException)
            {
                // Si la respuesta no puede ser deserializada como un error, se devuelve un objeto genérico.
                return new
                {
                    error = "Unexpected error",
                    details = responseBody
                };
            }
        }

    }
}
