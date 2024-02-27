using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtowerDocElectronico.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasMaestrosNubexGetController : ControllerBase
    {

        private readonly IConsultasMaestrosNubexGet _consultasNubext;

        public ConsultasMaestrosNubexGetController(IConsultasMaestrosNubexGet consultasNubext)
        {
            _consultasNubext = consultasNubext;
        }

        [HttpGet("ObtenerImpuestos")]
        public async Task<IActionResult> ObtenerImpuestos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null,string? descripcion = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (tipoDocumentos, totalPages) = await _consultasNubext.ObtenerImpuestos(page, pageSize, codigo, nombre, descripcion);

            if (tipoDocumentos == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = tipoDocumentos,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerDescuentos")]
        public async Task<IActionResult> ObtenerDescuentos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (tipoDocumentos, totalPages) = await _consultasNubext.ObtenerDescuentos(page, pageSize, codigo, nombre);

            if (tipoDocumentos == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = tipoDocumentos,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerMetodoPago")]
        public async Task<IActionResult> ObtenerMetodoPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (tipoDocumentos, totalPages) = await _consultasNubext.ObtenerMetodoPago(page, pageSize, codigo, nombre);

            if (tipoDocumentos == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = tipoDocumentos,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }
        [HttpGet("ObtenerFormasPago")]
        public async Task<IActionResult> ObtenerFormasPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (tipoDocumentos, totalPages) = await _consultasNubext.ObtenerFormasPago(page, pageSize, codigo, nombre);

            if (tipoDocumentos == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = tipoDocumentos,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerTipoDocumentos")]
        public async Task<IActionResult> ObtenerTipoDocumentos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (tipoDocumentos, totalPages) = await _consultasNubext.ObtenerTipoDocumentosPaginados(page, pageSize, codigo, nombre);

            if (tipoDocumentos == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = tipoDocumentos,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerOrganizaciones")]
        public async Task<IActionResult> ObtenerOrganizaciones(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (organizaciones, totalPages) = await _consultasNubext.ObtenerTipoOrganizacionPaginados(page, pageSize, codigo, nombre);

            if (organizaciones == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = organizaciones,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerRegimen")]
        public async Task<IActionResult> ObtenerRegimen(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (regimen, totalPages) = await _consultasNubext.ObtenerTipoRegimenPaginados(page, pageSize, codigo, nombre);

            if (regimen == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = regimen,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }
        [HttpGet("ObtenerResponsabilidades")]
        public async Task<IActionResult> ObtenerResponsabilidades(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (responsabilidad, totalPages) = await _consultasNubext.ObtenerTipoResponsabilidadPaginados(page, pageSize, codigo, nombre);

            if (responsabilidad == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = responsabilidad,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        [HttpGet("ObtenerCiudades")]
        public async Task<IActionResult> ObtenerCiudades(int page = 1, int pageSize = 10, string? codigoCiudad = null, string? nombreCiudad = null, string? nombreDepartamento = null)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var (ciudades, totalPages) = await _consultasNubext.ObtenerCiudadPaginados(page, pageSize, codigoCiudad, nombreCiudad, nombreDepartamento);

            if (ciudades == null)
            {
                return NotFound();
            }

            var result = new
            {
                Data = ciudades,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(result);
        }
    }
}
