using System.Text.Json.Serialization;

namespace AtowerDocElectronico.Infraestructura.Entities
{
    public class Facturas
    {
        public int Id { get; set; }
        public required int IdCompany { get; set; }
        public required string Prefijo { get; set; }
        public required string NumeroFactura { get; set; }
        public required string Factura {  get; set; }
        public int? Estado { get; set; } = 0;
        public string? FechaFactura {  get; set; }        
        public string? NitEmpresa { get; set; }
        public string? DocumentoAdquisiente { get; set; }
        public decimal? ValorFactura { get; set; }
        public decimal? ValorIva {  get; set; }
        public decimal? ValorOtro { get; set; }
        public decimal? ValorTotal {  get; set; }
        public string? Cufe {  get; set; }
        public string? Contrato {  get; set; }
        public string? DireccionFacturaDian {  get; set; }
        public byte[]? Base64Pdf { get; set; }
        public string? JsonEnvioAtower { get; set; }
        public string? JsonEnvioNubex { get; set; }
        public string? JsonRespuestaNubex { get; set; }

        [JsonIgnore]
        public Compañia? Compañia { get; set; }

    }
}
