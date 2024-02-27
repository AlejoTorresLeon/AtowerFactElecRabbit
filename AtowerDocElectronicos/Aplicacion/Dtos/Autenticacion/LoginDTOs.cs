using System.ComponentModel;

namespace AtowerDocElectronico.Aplicacion.Dtos.Autenticacion
{
    public class LoginDTOs
    {        

        public string Email { get; set; } = null!;

        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
}
