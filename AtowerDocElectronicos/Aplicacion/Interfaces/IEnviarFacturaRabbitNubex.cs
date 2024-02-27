using AtowerDocElectronico.Aplicacion.Dtos.Factura;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface IEnviarFacturaRabbitNubex
    {
        Task<FacturaNubexDto> EnviarFacturaRabbit(FacturaAtowerDTO fasturaAtower, int idCompañia);
    }
}
