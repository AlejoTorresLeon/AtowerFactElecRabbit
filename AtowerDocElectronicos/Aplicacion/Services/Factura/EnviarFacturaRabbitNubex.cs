using AtowerDocElectronico.Aplicacion.Dtos.Factura;
using AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Aplicacion.Validations;
using AtowerDocElectronico.Infraestructura.Entities;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.Dtos;
using RabbitMQEventBus.EventoQueue;
using System.Data;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AtowerDocElectronico.Aplicacion.Services.Factura
{
    public class EnviarFacturaRabbitNubex: IEnviarFacturaRabbitNubex
    {
        private readonly INubexDbContext _nubexDbContext;
        private readonly IRabbitEventBus _rabbitEventBus;
        private readonly IAtowerDbContext _atowerDbContext;
        public EnviarFacturaRabbitNubex(INubexDbContext nubexDbContext, IRabbitEventBus rabbitEventBus, IAtowerDbContext atowerDbContext)
        {
            _nubexDbContext = nubexDbContext;
            _rabbitEventBus = rabbitEventBus;
            _atowerDbContext = atowerDbContext;
        }

        public async Task<ResponseGenericDtos> EnviarFacturaRabbit(FacturaAtowerDTO facturaAtower, int idUsuario)
        {
            var response = new ResponseGenericDtos();

            var errores = ValidarCamposObligatorios(facturaAtower);

            if (errores.Count > 0)
            {
                response.Success = false;
                response.Message = "Hay campos obligatorios faltantes";
                response.Errors = errores;
                return response;
            }

            try
            {
                var idCompañia = await _atowerDbContext.Compañias
                                        .Where(c => c.IdUsuarioCreador == idUsuario)
                                        .Select(c => c.IdCompanyNubex)
                                        .FirstOrDefaultAsync();

                var facturaNubex = await TransformacionFactura(facturaAtower, (int)idCompañia);

                var facturaRabbit = TransformacionRabbit(facturaNubex);

                // Realizar la validación de los totales
                if (ValidarTotales(facturaAtower, facturaNubex, out List<string> totalesErrores))
                {
                    _rabbitEventBus.Publish(new FacturaEventoQueue(facturaRabbit, (int?)idCompañia));
                    response.Success = true;
                    response.Message = "La factura se envió correctamente.";
                    response.Data = facturaRabbit;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Los valores de la factura no coinciden:";
                    response.Errors = totalesErrores;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Se produjo un error al enviar la factura.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }


        private List<string> ValidarCamposObligatorios(FacturaAtowerDTO facturaAtower)
        {
            var errores = new List<string>();

            if (string.IsNullOrEmpty(facturaAtower.Numero_factura))
                errores.Add("El campo 'numero_factura' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.Fecha))
                errores.Add("El campo 'fecha' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.Hora))
                errores.Add("El campo 'hora' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.Prefijo))
                errores.Add("El campo 'prefijo' es obligatorio.");

            if (facturaAtower.EnviarCorreo == null)
                errores.Add("El campo 'enviarCorreo' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.DetalleGeneral))
                errores.Add("El campo 'detalleGeneral' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.DetalleCabecera))
                errores.Add("El campo 'detalleCabecera' es obligatorio.");

            if (string.IsNullOrEmpty(facturaAtower.DetallePieHoja))
                errores.Add("El campo 'detallePieHoja' es obligatorio.");



            if (facturaAtower.Cliente == null)
                errores.Add("El campo 'cliente' es obligatorio.");
            else
            {
                if (string.IsNullOrEmpty(facturaAtower.Cliente.Identificacion))
                    errores.Add("El campo 'identificacion' del cliente es obligatorio.");

                if (facturaAtower.Cliente.Dv == null)
                    errores.Add("El campo 'dv' del cliente es obligatorio.");

                if (string.IsNullOrEmpty(facturaAtower.Cliente.Nombre))
                    errores.Add("El campo 'nombre' del cliente es obligatorio.");

                if (facturaAtower.Cliente.Telefono == null)
                    errores.Add("El campo 'telefono' del cliente es obligatorio.");

                if (string.IsNullOrEmpty(facturaAtower.Cliente.Direccion))
                    errores.Add("El campo 'direccion' del cliente es obligatorio.");

                if (string.IsNullOrEmpty(facturaAtower.Cliente.Correo))
                    errores.Add("El campo 'correo' del cliente es obligatorio.");

                if (facturaAtower.Cliente.IdTipoDocumento == null)
                    errores.Add("El campo 'idTipoDocumento' del cliente es obligatorio.");

                if (facturaAtower.Cliente.IdTipoOrganizacion == null)
                    errores.Add("El campo 'idTipoOrganizacion' del cliente es obligatorio.");

                if (facturaAtower.Cliente.IdTipoRegimen == null)
                    errores.Add("El campo 'idTipoRegimen' del cliente es obligatorio.");

                if (facturaAtower.Cliente.IdTipoResponsabilidad == null)
                    errores.Add("El campo 'idTipoResponsabilidad' del cliente es obligatorio.");

                if (facturaAtower.Cliente.IdCiudad == null)
                    errores.Add("El campo 'idCiudad' del cliente es obligatorio.");
            }

            if (facturaAtower.FormaPago == null)
                errores.Add("El campo 'FormaPago' es obligatorio.");
            else
            {
                if (facturaAtower.FormaPago.IdFormaPago == null)
                    errores.Add("El campo 'idFormaPago' de formaPago es obligatorio.");

                if (facturaAtower.FormaPago.IdMetodoPago == null)
                    errores.Add("El campo 'idMetodoPago' de formaPago es obligatorio.");

                if (string.IsNullOrEmpty(facturaAtower.FormaPago.FechaPago))
                    errores.Add("El campo 'fechaPago' de formaPago es obligatorio.");

                if (facturaAtower.FormaPago.Duracion == null)
                    errores.Add("El campo 'duracion' de formaPago es obligatorio.");
            }

            if (facturaAtower.Cargos == null || facturaAtower.Cargos.Count == 0)
                errores.Add("Debe proporcionar al menos un cargo.");
            else
            {
                foreach (var cargo in facturaAtower.Cargos)
                {
                    if (cargo.Cantidad == null)
                        errores.Add("El campo 'Cantidad' de Cargos es obligatorio.");

                    if (cargo.ValorCargo == null)
                        errores.Add("El campo 'valorCargo' de Cargos es obligatorio.");

                    if (string.IsNullOrEmpty(cargo.Descripcion))
                        errores.Add("El campo 'descripcion' de Cargos es obligatorio.");

                    if (string.IsNullOrEmpty(cargo.Nota))
                        errores.Add("El campo 'nota' de Cargos es obligatorio.");

                    if (string.IsNullOrEmpty(cargo.Codigo))
                        errores.Add("El campo 'codigo' de Cargos es obligatorio.");

                    if (cargo.ImpuestoCargo != null && cargo.ImpuestoCargo.Count > 0)
                    {
                        foreach (var impuesto in cargo.ImpuestoCargo)
                        {
                            if (impuesto.IdTipoImpuesto == null)
                                errores.Add("El campo 'idTipoImpuesto' de Impuesto Cargo es obligatorio.");

                            if (impuesto.ValorImpuesto == null)
                                errores.Add("El campo 'valorImpuesto' de Impuesto Cargo es obligatorio.");

                            if (impuesto.ValorBaseImpuesto == null)
                                errores.Add("El campo 'valorBaseImpuesto' de Impuesto Cargo es obligatorio.");

                            if (impuesto.Porcentaje == null)
                                errores.Add("El campo 'porcentaje' de Impuesto Cargo es obligatorio.");

                        }
                    }
                }
            }

            if (facturaAtower.ImpuestoTotales != null && facturaAtower.ImpuestoTotales.Count > 0)
            {
                foreach (var impuestoTotales in facturaAtower.ImpuestoTotales)
                {
                    if (impuestoTotales.IdTipoImpuesto == null)
                        errores.Add("El campo 'idTipoImpuesto' de ImpuestoTotales es obligatorio.");

                    if (impuestoTotales.ValorImpuesto == null)
                        errores.Add("El campo 'valorImpuesto' de ImpuestoTotales es obligatorio.");

                    if (impuestoTotales.Porcentaje == null)
                        errores.Add("El campo 'valorImpuesto' de ImpuestoTotales es obligatorio.");

                    if (impuestoTotales.ValorBaseImpuesto == null)
                        errores.Add("El campo 'valorBaseImpuesto' de ImpuestoTotales es obligatorio.");
                }
            }

            if (facturaAtower.TotalesNeto == null)
                errores.Add("Debe proporcionar los totlaesNeto.");
            else
            {
                if (facturaAtower.TotalesNeto.TotalCargos == null)
                    errores.Add("El campo 'totalCargos' de totalesNeto es obligatorio.");

                if (facturaAtower.TotalesNeto.TotalDescuento == null)
                    errores.Add("El campo 'totalDescuento' de totalesNeto es obligatorio.");

                if (facturaAtower.TotalesNeto.TotalPagar == null)
                    errores.Add("El campo 'totalPagar' de totalesNeto es obligatorio.");
            }

            return errores;
        }


        private bool ValidarTotales(FacturaAtowerDTO facturaAtower, FacturaNubexDto facturaNubex, out List<string> errores)
        {
            errores = new List<string>();

            // Validar los totales de la factura
            decimal totalCargosLineas = (facturaAtower.Cargos != null) ? facturaAtower.Cargos.Sum(c => c.ValorCargo ?? 0) : 0;
            decimal totalCargosTotal = (facturaAtower.TotalesNeto != null) ? (decimal)facturaAtower.TotalesNeto.TotalCargos : 0;
            decimal totalDescuentosGenerales = (facturaAtower.DescuentosGenerales != null) ? facturaAtower.DescuentosGenerales.Sum(d => d.ValorDescuento ?? 0) : 0;
            decimal totalImpuestos = (facturaAtower.ImpuestoTotales != null) ? facturaAtower.ImpuestoTotales.Sum(i => i.ValorImpuesto ?? 0) : 0;
            decimal totalImpuestoCargosLineas =(facturaAtower.Cargos != null) ? (facturaAtower.Cargos != null) ? facturaAtower.Cargos.Sum(c => (c.ImpuestoCargo != null) ? c.ImpuestoCargo.Sum(i => i.ValorImpuesto ?? 0) : 0) : 0 : 0;
            decimal totalApagar = (facturaAtower.TotalesNeto != null) ? (decimal)facturaAtower.TotalesNeto.TotalPagar : 0;

            decimal totalAPagarEsperado = (totalCargosTotal - totalDescuentosGenerales) + (totalImpuestos);


            if (totalCargosLineas != totalCargosTotal)
            {
                errores.Add($"El total de cargos total ({totalCargosTotal}) no coincide con la suma de los cargos entre lineas ({totalCargosLineas}).");
            }
            
            if (totalImpuestos != totalImpuestoCargosLineas)
            {
                errores.Add($"El total de impuesto total ({totalImpuestos}) no coincide con la suma de los impuestos entre lineas de cargos ({totalImpuestoCargosLineas}).");
            }


            if (totalAPagarEsperado != totalApagar)
            {
                errores.Add($"El total a pagar ({totalApagar}) no coincide con el cálculo esperado de los totales ({totalAPagarEsperado}).");
            }


            return errores.Count == 0;
        }
        private FacturaNubex TransformacionRabbit(FacturaNubexDto factura)
        {
            var facturaNubex = new FacturaNubex();

            // Asignación de propiedades una por una
            facturaNubex.number = factura.number;
            facturaNubex.type_document_id = factura.type_document_id;
            facturaNubex.date = factura.date;
            facturaNubex.time = factura.time;
            facturaNubex.resolution_number = factura?.resolution_number;
            facturaNubex.prefix = factura?.prefix;
            facturaNubex.notes = factura?.notes;
            facturaNubex.head_note = factura?.head_note;
            facturaNubex.foot_note = factura?.foot_note;
            facturaNubex.sendmail = factura?.sendmail;
            facturaNubex.sendmailtome = factura?.sendmailtome;

            // Asignación de la propiedad customer
            facturaNubex.customer = new RabbitMQEventBus.Dtos.ClienteDTO
            {
                identification_number = factura?.customer?.identification_number,
                dv = factura?.customer?.dv,
                name = factura?.customer?.name,
                phone = factura?.customer?.phone,
                address = factura?.customer?.address,
                email = factura?.customer?.email,
                merchant_registration = factura?.customer?.merchant_registration,
                type_document_identification_id = factura?.customer?.type_document_identification_id,
                type_organization_id = factura?.customer?.type_organization_id,
                type_regime_id = factura?.customer?.type_regime_id,
                type_liability_id = factura?.customer?.type_liability_id,
                municipality_id = factura?.customer?.municipality_id
            };

            // Asignación de la propiedad payment_form
            facturaNubex.payment_form = new RabbitMQEventBus.Dtos.FormaPagoDTO
            {
                payment_form_id = factura?.payment_form?.payment_form_id,
                payment_method_id = factura?.payment_form?.payment_method_id,
                payment_due_date = factura?.payment_form?.payment_due_date,
                duration_measure = factura?.payment_form?.duration_measure
            };

            // Asignación de la propiedad allowance_charges
            facturaNubex.allowance_charges = factura?.allowance_charges?
                .Select(r => new RabbitMQEventBus.Dtos.DescuentoGeneralDTO
            {
                discount_id = r.discount_id,
                charge_indicator = r.charge_indicator,
                allowance_charge_reason = r.allowance_charge_reason,
                amount = r.amount,
                base_amount = r.base_amount
            }).ToList();

            // Asignación de la propiedad invoice_lines
            facturaNubex.invoice_lines = factura?.invoice_lines?
                .Select(c => new RabbitMQEventBus.Dtos.CargoDTO
                {
                    unit_measure_id = c.unit_measure_id,
                    invoiced_quantity = c.invoiced_quantity,
                    line_extension_amount = c.line_extension_amount,
                    description = c.description,
                    notes = c.notes,
                    free_of_charge_indicator = c.free_of_charge_indicator,
                    code = c.code,
                    tax_totals = c.tax_totals != null
                        ? c.tax_totals.Select(p => new RabbitMQEventBus.Dtos.ImpuestoCargoDTO
                        {
                            tax_id = p.tax_id,
                            tax_amount = p.tax_amount,
                            taxable_amount = p.taxable_amount,
                            percent = p.percent
                        }).ToList()
                        : new List<RabbitMQEventBus.Dtos.ImpuestoCargoDTO>(),
                    base_quantity = c.base_quantity,
                    type_item_identification_id = c.type_item_identification_id,
                    price_amount = c.price_amount
                }).ToList();

            // Asignación de la propiedad tax_totals
            facturaNubex.tax_totals = factura?.tax_totals?.Select(i => new RabbitMQEventBus.Dtos.ImpuestoTotalDTO
            {
                tax_id = i.tax_id,
                tax_amount = i.tax_amount,
                percent = i.percent,
                taxable_amount = i.taxable_amount
            }).ToList();

            // Asignación de la propiedad legal_monetary_totals
            facturaNubex.legal_monetary_totals = new RabbitMQEventBus.Dtos.LegalMonetaryTotalsDTO
            {
                line_extension_amount = factura?.legal_monetary_totals?.line_extension_amount,
                tax_exclusive_amount = factura?.legal_monetary_totals?.tax_exclusive_amount,
                tax_inclusive_amount = factura?.legal_monetary_totals?.tax_inclusive_amount,
                allowance_total_amount = factura?.legal_monetary_totals?.allowance_total_amount,
                payable_amount = factura?.legal_monetary_totals?.payable_amount
            };

            return facturaNubex;
        }
        private async Task<FacturaNubexDto> TransformacionFactura(FacturaAtowerDTO facturadto, int idCompañia)
        {
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            var sql = "select resolution as ResolutionNumber , prefix as Prefix  from resolutions r where company_id  = @CompanyId and type_document_id  = 1 limit 1";
            var documentos = await dbConnection.QueryFirstOrDefaultAsync<ResolutionFacturaDto>(sql, new { CompanyId = idCompañia });

            var taxExclusiveAmount = facturadto?.ImpuestoTotales?.Sum(i => i.ValorImpuesto) ?? 0;

            var legalMonetaryTotalsTaxExclusiveAmount = taxExclusiveAmount != 0
                ? facturadto?.TotalesNeto?.TotalCargos
                : 0;


            return new FacturaNubexDto
            {
                number = facturadto.Numero_factura,
                type_document_id = "1",
                date = facturadto.Fecha,
                time = facturadto.Hora,
                resolution_number = documentos?.ResolutionNumber,
                prefix = documentos?.Prefix,
                notes = facturadto.DetalleGeneral,
                head_note = facturadto.DetalleCabecera,
                foot_note = facturadto.DetallePieHoja,
                sendmail = facturadto.EnviarCorreo,
                sendmailtome = facturadto.EnviarCorreo,
                customer = new Dtos.Factura.ClienteDTO
                {
                    identification_number = facturadto?.Cliente?.Identificacion,
                    dv = facturadto?.Cliente?.Dv,
                    name = facturadto?.Cliente?.Nombre,
                    phone = facturadto?.Cliente?.Telefono.ToString(),
                    address = facturadto?.Cliente?.Direccion,
                    email = facturadto?.Cliente?.Correo,
                    merchant_registration = "0000000-00",
                    type_document_identification_id = facturadto?.Cliente?.IdTipoDocumento,
                    type_organization_id = facturadto?.Cliente?.IdTipoOrganizacion,
                    type_regime_id = facturadto?.Cliente?.IdTipoRegimen,
                    type_liability_id = facturadto?.Cliente?.IdTipoResponsabilidad,
                    municipality_id = facturadto?.Cliente?.IdCiudad
                },
                payment_form = new Dtos.Factura.FormaPagoDTO
                {
                    payment_form_id = facturadto?.FormaPago?.IdFormaPago,
                    payment_method_id = facturadto?.FormaPago?.IdMetodoPago,
                    payment_due_date = facturadto?.FormaPago?.FechaPago,
                    duration_measure = facturadto?.FormaPago?.Duracion
                },
                allowance_charges = facturadto?.DescuentosGenerales?.Select(r => new Dtos.Factura.DescuentoGeneralDTO
                {
                    discount_id = r.IdTipoDescuento,
                    charge_indicator = false,
                    allowance_charge_reason = r.Descripcion,
                    amount = r.ValorDescuento,
                    base_amount = r.ValorBaseDescuento
                }).ToList(),
                invoice_lines = facturadto?.Cargos?.Select(c => new Dtos.Factura.CargoDTO
                {
                    unit_measure_id = 70,
                    invoiced_quantity = c.Cantidad,
                    line_extension_amount = c.ValorCargo,
                    description = c.Descripcion,
                    notes = c.Nota,
                    free_of_charge_indicator = false,
                    code = c.Codigo,
                    tax_totals = c.ImpuestoCargo?.Select(p => new Dtos.Factura.ImpuestoCargoDTO
                    {
                        tax_id = p.IdTipoImpuesto,
                        tax_amount = p.ValorImpuesto,
                        taxable_amount = p.ValorBaseImpuesto,
                        percent = p.Porcentaje
                    }).ToList(),
                    base_quantity = "1",
                    type_item_identification_id = "4",
                    price_amount = c.ValorNeto
                }).ToList(),
                tax_totals = facturadto?.ImpuestoTotales?.Select(i => new Dtos.Factura.ImpuestoTotalDTO
                {
                    tax_id = i.IdTipoImpuesto,
                    tax_amount = i.ValorImpuesto,
                    percent = i.Porcentaje,
                    taxable_amount = i.ValorBaseImpuesto
                }).ToList(),                

                legal_monetary_totals = new Dtos.Factura.LegalMonetaryTotalsDTO
                {
                    line_extension_amount = facturadto?.TotalesNeto?.TotalCargos,
                    tax_exclusive_amount = taxExclusiveAmount != 0 ? facturadto?.TotalesNeto?.TotalCargos : 0,
                    tax_inclusive_amount = facturadto?.TotalesNeto?.TotalCargos + (taxExclusiveAmount != 0 ? taxExclusiveAmount : 0) ,
                    allowance_total_amount = facturadto?.TotalesNeto?.TotalDescuento,
                    payable_amount = facturadto?.TotalesNeto?.TotalPagar
                }

            };

        }
    }
}
