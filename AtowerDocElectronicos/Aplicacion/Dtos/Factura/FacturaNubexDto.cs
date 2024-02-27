namespace AtowerDocElectronico.Aplicacion.Dtos.Factura
{
    public class FacturaNubexDto
    {
        public string? number { get; set; }
        public string? type_document_id { get; set; }
        public string? date { get; set; }
        public string? time { get; set; }
        public string? resolution_number { get; set; }
        public string? prefix { get; set; }
        public string? notes { get; set; }
        public string? head_note { get; set; }
        public string? foot_note { get; set; }
        public bool? sendmail { get; set; }
        public bool? sendmailtome { get; set; }
        public ClienteDTO? customer { get; set; }
        public FormaPagoDTO? payment_form { get; set; }
        public List<DescuentoGeneralDTO>? allowance_charges { get; set; }
        public List<CargoDTO>? invoice_lines { get; set; }
        public List<ImpuestoTotalDTO>? tax_totals { get; set; }
        public LegalMonetaryTotalsDTO? legal_monetary_totals { get; set; }
    }

    public class ClienteDTO
    {
        public string? identification_number { get; set; }
        public int? dv { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
        public string? merchant_registration { get; set; }
        public int? type_document_identification_id { get; set; }
        public int? type_organization_id { get; set; }
        public int? type_regime_id { get; set; }
        public int? type_liability_id { get; set; }
        public int? municipality_id { get; set; }
    }

    public class FormaPagoDTO
    {
        public int? payment_form_id { get; set; }
        public int? payment_method_id { get; set; }
        public string? payment_due_date { get; set; }
        public int? duration_measure { get; set; }
    }

    public class DescuentoGeneralDTO
    {
        public int? discount_id { get; set; }
        public bool? charge_indicator { get; set; }
        public string? allowance_charge_reason { get; set; }
        public decimal? amount { get; set; }
        public decimal? base_amount { get; set; }
    }

    public class CargoDTO
    {
        public int? unit_measure_id { get; set; }
        public int? invoiced_quantity { get; set; }
        public decimal? line_extension_amount { get; set; }
        public string? description { get; set; }
        public string? notes { get; set; }
        public bool? free_of_charge_indicator { get; set; }
        public string? code { get; set; }
        public List<ImpuestoCargoDTO>? tax_totals { get; set; }
        public string? base_quantity { get; set; }
        public string? type_item_identification_id { get; set; }
        public decimal? price_amount { get; set; }
    }

    public class ImpuestoCargoDTO
    {
        public int? tax_id { get; set; }
        public decimal? tax_amount { get; set; }
        public decimal? taxable_amount { get; set; }
        public float? percent { get; set; }
    }

    public class ImpuestoTotalDTO
    {
        public int? tax_id { get; set; }
        public decimal? tax_amount { get; set; }
        public float? percent { get; set; }
        public decimal? taxable_amount { get; set; }
    }

    public class LegalMonetaryTotalsDTO
    {
        public decimal? line_extension_amount { get; set; }
        public decimal? tax_exclusive_amount { get; set; }
        public decimal? tax_inclusive_amount { get; set; }
        public decimal? allowance_total_amount { get; set; }
        public decimal? payable_amount { get; set; }
    }
}
