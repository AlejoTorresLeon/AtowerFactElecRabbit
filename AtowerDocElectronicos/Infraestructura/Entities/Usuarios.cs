using System.Text.Json.Serialization;

namespace AtowerDocElectronico.Infraestructura.Entities
{
    public class Usuarios
    {
        public int Id { get; set; }
        public required ulong Identificacion { get; set; }
        public required string Nombre { get; set; }        
        public required string Email { get; set; }        
        public required int IdRol {  get; set; }
        public required bool Bloqueo {  get; set; }
        public required string PasswordSalt { get; set; }
        public required string PasswordHash { get; set; }

        [JsonIgnore]
        public  Roles? Roles { get; set; }
    }
}
