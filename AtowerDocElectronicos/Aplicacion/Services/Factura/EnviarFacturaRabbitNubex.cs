using AtowerDocElectronico.Aplicacion.Dtos.Factura;
using AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros;
using AtowerDocElectronico.Aplicacion.Interfaces;
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

        public async Task<FacturaNubexDto> EnviarFacturaRabbit(FacturaAtowerDTO fasturaAtower,int idUsuario)
        {

            var idCompañia = await _atowerDbContext.Compañias
                                    .Where(c => c.IdUsuarioCreador == idUsuario)
                                    .Select(c => c.IdCompanyNubex)
                                    .FirstOrDefaultAsync();


            var facturaNubex = await TransformacionFactura(fasturaAtower, (int)idCompañia);

            var facturaRabbit = TransformacionRabbit(facturaNubex);

            _rabbitEventBus.Publish(new FacturaEventoQueue(facturaRabbit, (int?)idCompañia));
            return facturaNubex;
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
                tax_totals = c.tax_totals?
                .Select(p => new RabbitMQEventBus.Dtos.ImpuestoCargoDTO
                {
                    tax_id = p.tax_id,
                    tax_amount = p.tax_amount,
                    taxable_amount = p.taxable_amount,
                    percent = p.percent
                }).ToList(),
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


        //private FacturaNubex TransformacionRabbit(FacturaNubexDto factura)
        //{
        //    var facturaNubex = new FacturaNubex();

        //    var properties = typeof(FacturaNubexDto).GetProperties();

        //    foreach (var property in properties)
        //    {                
        //        var value = property.GetValue(factura);

        //        var correspondingProperty = typeof(FacturaNubex).GetProperty(property.Name);

        //        if (correspondingProperty != null)
        //        {
        //            correspondingProperty.SetValue(facturaNubex, value);
        //        }
        //    }

        //    return facturaNubex;
        //}


        private async Task<FacturaNubexDto> TransformacionFactura(FacturaAtowerDTO facturadto, int idCompañia)
        {
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            var sql = "select resolution as ResolutionNumber , prefix as Prefix  from resolutions r where company_id  = @CompanyId and type_document_id  = 1 limit 1";
            var documentos = await dbConnection.QueryFirstOrDefaultAsync<ResolutionFacturaDto>(sql, new { CompanyId = idCompañia });


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
                    tax_exclusive_amount = facturadto?.TotalesNeto?.TotalCargos,
                    tax_inclusive_amount = facturadto?.TotalesNeto?.TotalImpuestoIncluido,
                    allowance_total_amount = facturadto?.TotalesNeto?.TotalDescuento,
                    payable_amount = facturadto?.TotalesNeto?.TotalPagar
                }

            };

        }
    }
}
