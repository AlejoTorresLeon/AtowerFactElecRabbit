using System.Text.Json.Serialization;

namespace AtowerDocElectronico.Infraestructura.Entities
{
    public class Compañia
    {
        public int Id { get; set; }
        public required string DigitoVerificacion { get; set; }
        public required string Identificacion { get; set; }
        public required string RazonSocial { get; set; }
        public required string Email { get; set; }
        public required ulong IdUsuarioNubex { get; set; }
        public required ulong IdCompanyNubex { get; set; }
        public required string TokenNubex { get; set; }
        public required bool Habilitado { get; set; }
        public required bool Bloqueo { get; set; }
        public required int IdUsuarioCreador { get; set; }

        [JsonIgnore] 
        public Usuarios? Usuarios { get; set; }

    }
}
