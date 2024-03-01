using AtowerDocElectronico.Aplicacion.Dtos.Autenticacion;
using AtowerDocElectronico.Infraestructura.Entities;

namespace AtowerDocElectronico.Aplicacion.Interfaces
{
    public interface ILogin
    {
        Task<TokenResponse?> GetUsuario(LoginDTOs usuarioDto);
        string? GetTokenIdUsuario(string token);
        //string GenerateSalt();
        //string GeneratePasswordHash(string password, byte[] salt);
        Task<Usuarios?> CreateUser(CrearUsuario usuarioNew);
    }
}
