using RabbitMQEventBus.Dtos;

namespace AtowerDocElectronico.Aplicacion.Dtos.Factura
{
    public class FacturaAtowerDto_Base64pdf
    {
        public FacturaAtowerDTO FacturaAtowerDTO { get; set; }
        public string? Base64Pdf { get; set; }

        public FacturaAtowerDto_Base64pdf(FacturaAtowerDTO facturaAtowerDTO, string? base64Pdf)
        {
            FacturaAtowerDTO = facturaAtowerDTO;
            Base64Pdf = base64Pdf;
        }
    }
}
