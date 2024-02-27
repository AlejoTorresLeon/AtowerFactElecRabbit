namespace AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros
{
    public class CiudadDepartamentoDTOs
    {
        public ulong Id { get; set; }
        public string CodigoCiudad { get; set; } = null!;

        public string NombreCiudad { get; set; } = null!;

        public string NombreDepartamento { get; set; } = null!;
    }
}
