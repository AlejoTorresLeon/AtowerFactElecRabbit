using AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface IConsultasMaestrosNubexGet
    {        
        Task<(IEnumerable<TipoResponsabilidadDTOs>, int)> ObtenerTipoResponsabilidadPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<CiudadDepartamentoDTOs>, int)> ObtenerCiudadPaginados(int page = 1, int pageSize = 10, string? codigoCiudad = null, string? nombreCiudad = null, string? nombreDepartamento = null);

        Task<(IEnumerable<TipoDocumentoDTOs>, int)> ObtenerTipoDocumentosPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<TipoOrganizacionDTOs>, int)> ObtenerTipoOrganizacionPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<TipoRegimenDTOs>, int)> ObtenerTipoRegimenPaginados(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);        
        Task<(IEnumerable<FormaPagoDTOs>, int)> ObtenerFormasPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<MetodoPagoDTOs>, int)> ObtenerMetodoPago(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<DescuentoDTOs>, int)> ObtenerDescuentos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null);
        Task<(IEnumerable<ImpuestosDTOs>, int)> ObtenerImpuestos(int page = 1, int pageSize = 10, string? codigo = null, string? nombre = null, string? descripcion = null);
    }
}
