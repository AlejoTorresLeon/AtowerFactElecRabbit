using AtowerDocElectronico.Aplicacion.Dtos.Autenticacion;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Infraestructura.Entities;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AtowerDocElectronico.Aplicacion.Services.Autenticacion
{
    public class Login : ILogin
    {
        private readonly IConfiguration _configuration;
        private readonly double _jwtExpirationMinutes;
        private readonly IAtowerDbContext _atowerDbContext;
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public Login(IConfiguration configuration, IAtowerDbContext atowerDbContext)
        {
            _configuration = configuration;
            _jwtExpirationMinutes = 7 * 24 * 60; // 7 días en minutos
            _atowerDbContext = atowerDbContext;
        }

        public async Task<Usuarios?> CreateUser(CrearUsuario usuarioNew)
        {
            if (usuarioNew == null)
            {
                throw new Exception($"Valdidacion");
            }

            var saltBytes = GenerateSalt();

            if (saltBytes == null)
            {
                throw new Exception($"Valdidacion");
            }
            string passwordHash = GeneratePasswordHash(usuarioNew.Password, Convert.FromBase64String(saltBytes));

           
            var usuarioGuardar = new Usuarios
            {
                Identificacion = usuarioNew.Identificacion,
                Nombre = usuarioNew.Nombre,
                Email= usuarioNew.Email,
                IdRol = (int)usuarioNew.IdRol,
                Bloqueo = false,
                PasswordSalt = saltBytes,
                PasswordHash = passwordHash
            };

           _atowerDbContext.Usuarios.Add(usuarioGuardar);
            await _atowerDbContext.SaveChangesAsync();

            return usuarioGuardar;
        }

        public async Task<TokenResponse?> GetUsuario(LoginDTOs usuarioDto)
        {
            var usuario = await _atowerDbContext.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioDto.Email);

            if (usuario != null)
            {
                string passwordHash = GeneratePasswordHash(usuarioDto.Password, Convert.FromBase64String(usuario.PasswordSalt));
                
                if (passwordHash == usuario.PasswordHash)
                {                   

                    var token = GenerateJwtToken(usuario);

                    var tokenResponse = new TokenResponse
                    {
                        Token = token,
                        Usuario = usuario.Nombre
                    };

                    return tokenResponse;

                }
            }

            return null;
        }


        private string? GenerateJwtToken(Usuarios user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Verificar si la configuración JWT:Key y JWT:EncryptionKey son nulas
            string? jwtKey = _configuration["JWT:Key"];
            string? encryptionKey = _configuration["JWT:EncryptionKey"];

            if (jwtKey == null || encryptionKey == null)
            {
                // Manejar el caso en el que las claves JWT no estén configuradas
                Console.WriteLine("Las claves JWT no están configuradas correctamente.");
                return null;
            }

            // Clave para firmar el token
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            // Clave para cifrar la carga útil
            var encryptionKeyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            // Verificar si encryptionKeyBytes es nulo
            if (encryptionKeyBytes == null)
            {
                // Manejar el caso en el que la clave de cifrado no se pudo obtener correctamente
                Console.WriteLine("Error al obtener la clave de cifrado.");
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim("IdUsuario", user.Id.ToString()),
                new Claim("IdRol", user.IdRol.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = new EncryptingCredentials(
                    new SymmetricSecurityKey(encryptionKeyBytes), // Usar la clave de cifrado obtenida
                    SecurityAlgorithms.Aes256KW,
                    SecurityAlgorithms.Aes256CbcHmacSha512)
            };

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Escribir el token
                var encryptedToken = tokenHandler.WriteToken(token);

                return encryptedToken;
            }
            catch (Exception ex)
            {
                // Maneja la excepción según tus necesidades
                Console.WriteLine($"Error al generar el token: {ex.Message}");
                return null;
            }
        }


        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string? jwtKey = _configuration["JWT:Key"];
            string? encryptionKey2 = _configuration["JWT:EncryptionKey"];

            if (jwtKey == null || encryptionKey2 == null)
            {
                // Manejar el caso en el que las claves JWT no estén configuradas
                Console.WriteLine("Las claves JWT no están configuradas correctamente.");
                return null;
            }


            var key = Encoding.UTF8.GetBytes(jwtKey);
            var encryptionKey = Encoding.UTF8.GetBytes(encryptionKey2);

            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey), // Agrega la clave de cifrado
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }

        public string? GetTokenIdUsuario(string token)
        {
            var claimsPrincipal = ValidateToken2(token);
            if (claimsPrincipal == null)
            {
                Console.WriteLine("Las claves JWT no están configuradas correctamente.");
                return null;
            }
            return claimsPrincipal;
        }

        private string? ValidateToken2(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string? jwtKey = _configuration["JWT:Key"];
            string? encryptionKey2 = _configuration["JWT:EncryptionKey"];

            if (jwtKey == null || encryptionKey2 == null)
            {
                // Manejar el caso en el que las claves JWT no estén configuradas
                Console.WriteLine("Las claves JWT no están configuradas correctamente.");
                return null;
            }


            var key = Encoding.UTF8.GetBytes(jwtKey);
            var encryptionKey = Encoding.UTF8.GetBytes(encryptionKey2);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey), // Agrega la clave de cifrado
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true // para asegurarse de que el token no esté vencido
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var connectionStringClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value;

            if (string.IsNullOrEmpty(connectionStringClaim))
                throw new SecurityTokenValidationException("El IdUsuario no se encontró en el token.");

            return connectionStringClaim;

        }




        private string GenerateSalt()
        {
            byte[] salt = new byte[64];

            rng.GetBytes(salt);

            // Convertir los bytes del salt a una cadena base64
            string saltString = Convert.ToBase64String(salt);

            return saltString;
        }

        private string GeneratePasswordHash(string password, byte[] salt)
        {

            using (var hmac = new HMACSHA512(salt))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }




    }
}
