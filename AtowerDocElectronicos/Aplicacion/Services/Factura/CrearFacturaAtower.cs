using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Infraestructura.Entities;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RabbitMQEventBus.Dtos;

namespace AtowerDocElectronico.Aplicacion.Services.Factura
{
    public class CrearFacturaAtower: IFacturaAtower
    {
        private readonly IAtowerDbContext _atowerDbContext;

        public CrearFacturaAtower(IAtowerDbContext atowerDbContext)
        {
            _atowerDbContext = atowerDbContext;
        }

        public async Task<Facturas?> GuardarOActualizarFactura(FacturaAtowerDTO facturaAtower,FacturaNubex facturaNubex, int idCompany,string? Base64Pdf = null)
        {
            try
            {
                string jsonFactura = JsonConvert.SerializeObject(facturaAtower);
                string jsonFacturaNubex = JsonConvert.SerializeObject(facturaNubex);
                byte[] pdfBytes = Convert.FromBase64String(Base64Pdf);
                // Buscar si la factura ya existe en la base de datos
                var facturaExistente = await _atowerDbContext.Facturas
                    .FirstOrDefaultAsync(f => f.Prefijo == facturaAtower.Prefijo && f.NumeroFactura == facturaAtower.Numero_factura && f.IdCompany == idCompany);

                var company = await _atowerDbContext.Compañias.FirstOrDefaultAsync(c => c.Id == idCompany);

                if (facturaExistente != null)
                {
                    // Actualizar los datos de la factura existente
                    facturaExistente.Prefijo = facturaAtower.Prefijo;
                    facturaExistente.NumeroFactura = facturaAtower.Numero_factura;
                    facturaExistente.Factura = facturaAtower.Prefijo + facturaAtower.Numero_factura;
                    facturaExistente.Estado = 0;
                    facturaExistente.FechaFactura = facturaAtower.Fecha;
                    facturaExistente.NitEmpresa = company?.Identificacion;
                    facturaExistente.DocumentoAdquisiente = facturaAtower?.Cliente?.Identificacion;
                    facturaExistente.ValorFactura = facturaAtower?.TotalesNeto?.TotalCargos;
                    facturaExistente.ValorIva = (facturaAtower?.ImpuestoTotales != null) ? facturaAtower.ImpuestoTotales.Sum(i => i.ValorImpuesto ?? 0) : 0;
                    facturaExistente.ValorOtro = 0;
                    facturaExistente.ValorTotal = facturaAtower?.TotalesNeto?.TotalPagar;                                                            
                    facturaExistente.Base64Pdf = pdfBytes;
                    facturaExistente.JsonEnvioAtower = jsonFacturaNubex;
                    facturaExistente.JsonEnvioNubex = jsonFactura;                    
                    // Guardar los cambios
                    await _atowerDbContext.SaveChangesAsync();

                    return facturaExistente;
                }
                else
                {
                    // Serializar el objeto FacturaAtowerDTO a JSON
                 

                    //// Crear un JObject a partir del JSON serializado
                    //JObject jsonEnvioAtower = JObject.Parse(jsonFactura);
                    //JObject jsonEnvioNubex = JObject.Parse(jsonFacturaNubex);

                    

                    var nuevaFactura = new Facturas
                    {
                        IdCompany = idCompany,
                        Prefijo = facturaAtower.Prefijo,
                        NumeroFactura = facturaAtower.Numero_factura,
                        Factura = facturaAtower.Prefijo + facturaAtower.Numero_factura,
                        Estado = 0,
                        FechaFactura = facturaAtower.Fecha,
                        NitEmpresa = company?.Identificacion,
                        DocumentoAdquisiente = facturaAtower?.Cliente?.Identificacion,
                        ValorFactura = facturaAtower?.TotalesNeto?.TotalCargos,
                        ValorIva = (facturaAtower?.ImpuestoTotales != null) ? facturaAtower.ImpuestoTotales.Sum(i => i.ValorImpuesto ?? 0) : 0,
                        ValorOtro = 0,
                        ValorTotal = facturaAtower?.TotalesNeto?.TotalPagar,
                        JsonEnvioAtower = jsonFactura,
                        JsonEnvioNubex = jsonFacturaNubex,
                        Base64Pdf = pdfBytes
                    };

                    // Agregar la nueva factura y guardar los cambios
                    _atowerDbContext.Facturas.Add(nuevaFactura);
                    await _atowerDbContext.SaveChangesAsync();

                    return nuevaFactura;
                }
            }
            catch (DbUpdateException ex)
            {
                // Accede a la excepción interna para obtener más detalles
                var innerException = ex.InnerException;
                // Aquí puedes manejar el error o lanzar una excepción personalizada
                // según lo que necesites hacer en tu aplicación.
                // Puedes registrar el mensaje de error interno, lanzar una excepción personalizada, etc.
                // Por ejemplo:
                throw new Exception("Ocurrió un error al guardar los cambios en la base de datos.", innerException);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Ocurrió un error al guardar los cambios en la base de datos.{ex.Message}");
            }

           
            
        }
        public async Task<Facturas?> ActualizarFacturasEnviadas(FacturaAtowerDTO facturaAtower, FacturaNubex facturaNubex, ResponseSimplificadoDian? response, int idCompany, string? Base64Pdf = null)
        {
            try
            {
                // Buscar si la factura ya existe en la base de datos
                var facturaExistente = await _atowerDbContext.Facturas
                    .FirstOrDefaultAsync(f => f.Prefijo == facturaAtower.Prefijo && f.NumeroFactura == facturaAtower.Numero_factura && f.IdCompany == idCompany);
                        string jsonFactura = JsonConvert.SerializeObject(facturaAtower);
                        string jsonFacturaNubex = JsonConvert.SerializeObject(facturaNubex);
                        string jsonRespuestaNubex = JsonConvert.SerializeObject(response);
                        byte[] pdfBytes = Convert.FromBase64String(Base64Pdf);

                if (facturaExistente != null)
                {
                    if (response?.Cufe != null)
                    {
                        //var company = await _atowerDbContext.Compañias.FirstOrDefaultAsync(c => c.Id == idCompany);


                        // Actualizar los datos de la factura existente
                        facturaExistente.Estado = 1;
                        facturaExistente.Cufe = response.Cufe;
                        facturaExistente.JsonRespuestaNubex = jsonRespuestaNubex;
                        // Guardar los cambios
                        await _atowerDbContext.SaveChangesAsync();

                        return facturaExistente;

                    }
                    else
                    {
                        // Actualizar los datos de la factura existente
                        facturaExistente.Estado = 2;                        
                        facturaExistente.JsonRespuestaNubex = jsonRespuestaNubex;
                        // Guardar los cambios
                        await _atowerDbContext.SaveChangesAsync();

                        return facturaExistente;

                    }
                }
                return facturaExistente;
            }
            catch (DbUpdateException ex)
            {
                // Accede a la excepción interna para obtener más detalles
                var innerException = ex.InnerException;
                // Aquí puedes manejar el error o lanzar una excepción personalizada
                // según lo que necesites hacer en tu aplicación.
                // Puedes registrar el mensaje de error interno, lanzar una excepción personalizada, etc.
                // Por ejemplo:
                throw new Exception("Ocurrió un error al guardar los cambios en la base de datos.", innerException);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Ocurrió un error al guardar los cambios en la base de datos.{ex.Message}");
            }



        }
    }
}
