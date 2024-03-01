using RabbitMQEventBus.Dtos;

namespace AtowerEnvioNubex.Aplicacion.Interfaces
{
    public interface IEnvioFacturaNubexDian
    {
        Task<ResponseSimplificadoDian?> EnviarFacturaNubexDian(FacturaNubex factura, int? IdCompañia);
    }
}
