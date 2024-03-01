using AtowerDocElectronico.Aplicacion.Dtos.Factura;
using AtowerDocElectronico.Aplicacion.Validations;
using RabbitMQEventBus.Dtos;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface IEnviarFacturaRabbitNubex
    {
        Task<ResponseGenericDtos> EnviarFacturaRabbit(FacturaAtowerDTO facturaAtower, int idUsuario,string? Base64Pdf = null);
    }
}
