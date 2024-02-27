using AtowerEnvioNubex.Aplicacion.Dtos;

namespace AtowerEnvioNubex.Aplicacion.Interfaces
{
    public interface IEnvioFacturaNubexDian
    {
        Task<ResponseSimplificadoDian?> EnviarFacturaNubexDian(FacturaDian factura, int? IdCompañia);
    }
}
