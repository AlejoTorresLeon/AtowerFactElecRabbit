namespace AtowerDocElectronico.Aplicacion.Services.Autenticacion
{
    public class UsuarioDTOs
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public ulong IdPermiso { get; set; }

    }
}
