using AtowerDocElectronico.Infraestructura.Entities;
using RabbitMQEventBus.Dtos;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface IFacturaAtower
    {
        Task<Facturas?> GuardarOActualizarFactura(FacturaAtowerDTO facturaAtower, FacturaNubex facturaNubex, int idCompany,string? Base64Pdf = null);
        Task<Facturas?> ActualizarFacturasEnviadas(FacturaAtowerDTO facturaAtower, FacturaNubex facturaNubex, ResponseSimplificadoDian? response, int idCompany, string? Base64Pdf = null);
    }
}
