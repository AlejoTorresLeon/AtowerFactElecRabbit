namespace AtowerDocElectronico.Aplicacion.Dtos.Factura
{
    public class ResponseFacturaAtowerDto
    {
        public string? Message { get; set; }
        public List<string>? Errores { get; set; }

        public string? Cufe { get; set; }
    }
}
