namespace AtowerEnvioNubex.Aplicacion.Dtos
{
    public class ResponseSimplificadoDian
    {        
        public int? IdCompañia { get; set; }
        public string? Factura { get; set; }
        public string? Message { get; set; }
        public List<string>? Errores { get; set; }

        public string? Cufe { get; set; }
    }
}
