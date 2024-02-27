using AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Validations;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface ICompañia
    {
        Task<ResponseGenericDtos?> CrearCompañia(CompañiaPostDto compani, int idUsuario);
    }
}
