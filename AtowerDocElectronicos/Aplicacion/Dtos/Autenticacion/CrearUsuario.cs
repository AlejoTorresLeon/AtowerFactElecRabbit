namespace AtowerDocElectronico.Aplicacion.Dtos.Autenticacion
{
    public class CrearUsuario
    {
        public required ulong Identificacion { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required ulong IdRol { get; set; }        
        public required string Password { get; set; }
    }
}
