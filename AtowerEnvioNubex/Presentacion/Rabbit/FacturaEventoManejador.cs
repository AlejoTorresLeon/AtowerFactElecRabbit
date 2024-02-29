using AtowerEnvioNubex.Aplicacion.Dtos;
using AtowerEnvioNubex.Aplicacion.Interfaces;
using RabbitMQEventBus.BusRabbit;
using RabbitMQEventBus.Dtos;
using RabbitMQEventBus.EventoQueue;
using RabbitMQEventBus.Implement;
using CargoDTO = AtowerEnvioNubex.Aplicacion.Dtos.CargoDTO;
using ClienteDTO = AtowerEnvioNubex.Aplicacion.Dtos.ClienteDTO;
using DescuentoGeneralDTO = AtowerEnvioNubex.Aplicacion.Dtos.DescuentoGeneralDTO;
using FormaPagoDTO = AtowerEnvioNubex.Aplicacion.Dtos.FormaPagoDTO;
using ImpuestoCargoDTO = AtowerEnvioNubex.Aplicacion.Dtos.ImpuestoCargoDTO;
using ImpuestoTotalDTO = AtowerEnvioNubex.Aplicacion.Dtos.ImpuestoTotalDTO;
using LegalMonetaryTotalsDTO = AtowerEnvioNubex.Aplicacion.Dtos.LegalMonetaryTotalsDTO;

namespace AtowerEnvioNubex.Presentacion.Rabbit
{
    public class FacturaEventoManejador : IEventoManejador<FacturaEventoQueue>
    {
        private readonly ILogger<FacturaEventoManejador> _logger;
        private readonly IEnvioFacturaNubexDian _envioFacturaNubexDian;
        private readonly IRabbitEventBus _rabbitEventBus;

        public FacturaEventoManejador(ILogger<FacturaEventoManejador> logger, IEnvioFacturaNubexDian envioFacturaNubexDian, IRabbitEventBus rabbitEventBus)
        {
            _logger = logger;
            _envioFacturaNubexDian = envioFacturaNubexDian;
            _rabbitEventBus = rabbitEventBus;
        }

        public FacturaEventoManejador() { }

        public async Task Handle(FacturaEventoQueue @event)
        {
            var factura =  TransformacionRabbit(@event.Factura);
            var idCompany = @event.IdCompañia;
            var response = await _envioFacturaNubexDian.EnviarFacturaNubexDian(factura, idCompany);

            if (response?.Cufe != null)
            {
                await Task.CompletedTask;
                var asunto = "Asunto normal";
                var cuerpo = "Body normal";
                _rabbitEventBus.Publish(new EmailEventoQueue("ajtorres@sismaerp.com", asunto, cuerpo));
                return;
            }
        }

        private  FacturaDian TransformacionRabbit(FacturaNubex factura)
        {
            var facturaNubex = new FacturaDian();

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
            facturaNubex.customer = new ClienteDTO
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
            facturaNubex.payment_form = new FormaPagoDTO
            {
                payment_form_id = factura?.payment_form?.payment_form_id,
                payment_method_id = factura?.payment_form?.payment_method_id,
                payment_due_date = factura?.payment_form?.payment_due_date,
                duration_measure = factura?.payment_form?.duration_measure
            };

            // Asignación de la propiedad allowance_charges
            facturaNubex.allowance_charges = factura?.allowance_charges?
                .Select(r => new DescuentoGeneralDTO
                {
                    discount_id = r.discount_id,
                    charge_indicator = r.charge_indicator,
                    allowance_charge_reason = r.allowance_charge_reason,
                    amount = r.amount,
                    base_amount = r.base_amount
                }).ToList();

            // Asignación de la propiedad invoice_lines
            facturaNubex.invoice_lines = factura?.invoice_lines?
                .Select(c => new CargoDTO
                {
                    unit_measure_id = c.unit_measure_id,
                    invoiced_quantity = c.invoiced_quantity,
                    line_extension_amount = c.line_extension_amount,
                    description = c.description,
                    notes = c.notes,
                    free_of_charge_indicator = c.free_of_charge_indicator,
                    code = c.code,
                    tax_totals = c.tax_totals?
                .Select(p => new ImpuestoCargoDTO
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
            facturaNubex.tax_totals = factura?.tax_totals?.Select(i => new ImpuestoTotalDTO
            {
                tax_id = i.tax_id,
                tax_amount = i.tax_amount,
                percent = i.percent,
                taxable_amount = i.taxable_amount
            }).ToList();

            // Asignación de la propiedad legal_monetary_totals
            facturaNubex.legal_monetary_totals = new LegalMonetaryTotalsDTO
            {
                line_extension_amount = factura?.legal_monetary_totals?.line_extension_amount,
                tax_exclusive_amount = factura?.legal_monetary_totals?.tax_exclusive_amount,
                tax_inclusive_amount = factura?.legal_monetary_totals?.tax_inclusive_amount,
                allowance_total_amount = factura?.legal_monetary_totals?.allowance_total_amount,
                payable_amount = factura?.legal_monetary_totals?.payable_amount
            };

            return facturaNubex;
        }
    }
}
