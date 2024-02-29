using AtowerDocElectronico.Aplicacion.Dtos.Factura;
using AtowerDocElectronico.Aplicacion.Validations;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface IEnviarFacturaRabbitNubex
    {
        Task<ResponseGenericDtos> EnviarFacturaRabbit(FacturaAtowerDTO facturaAtower, int idUsuario);
    }
}
