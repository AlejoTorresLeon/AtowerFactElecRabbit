using System.ComponentModel.DataAnnotations;

namespace AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex
{
    public class CompañiaPostDto
    {
        public required int Identificacion {  get; set; }
        public required int DigitoVerificacion { get; set; }
        public required int IdTipoIdentificacion { get; set; }
        public required int IdTipoOrganizacion { get; set; }
        public required int IdTipoRegimen { get; set; }
        public required int IdTipoResponsabilidad { get; set; }
        public required string NombreEmpresa { get; set; }
        public required string NumeroCamaraComercio { get; set; }
        public required int IdCiudad { get; set; }        
        public required string Direccion { get; set; }
        public required long Telefono { get; set; }

        [EmailAddress]
        public required string Correo { get; set; }
    }
}
