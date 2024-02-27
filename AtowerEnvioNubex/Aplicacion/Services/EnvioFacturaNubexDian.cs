using AtowerEnvioNubex.Aplicacion.Dtos;
using AtowerEnvioNubex.Aplicacion.Interfaces;
using AtowerEnvioNubex.Infraestructura.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AtowerEnvioNubex.Aplicacion.Services
{
    public class EnvioFacturaNubexDian: IEnvioFacturaNubexDian
    {
        private readonly IEnviarHttp _enviarHttp;
        private readonly INubexDbContext _nubexDbContext;
        private readonly IConfiguration _configuration;
        public EnvioFacturaNubexDian(IEnviarHttp enviarHttp, INubexDbContext nubexDbContext, IConfiguration configuration)
        {
            _enviarHttp = enviarHttp;
            _nubexDbContext = nubexDbContext;
            _configuration = configuration;
        }

        public async Task<ResponseSimplificadoDian?> EnviarFacturaNubexDian(FacturaDian factura,int? IdCompañia)
        {
            // Acceder a la conexión de base de datos a través de Entity Framework Core
            using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

            // Construir la consulta SQL base
            var sql = "select u.api_token  from companies c   inner join users u ON u.id = c.user_id  where c.id = @CompanyId;";

            var token = await dbConnection.QueryFirstOrDefaultAsync<string>(sql, new { CompanyId = IdCompañia });

            if (token == null)
            {
                return null;
            }

            var url = $"{_configuration["NubexApi"]}/api/ubl2.1/invoice";
            var content = new StringContent(JsonConvert.SerializeObject(factura), Encoding.UTF8, "application/json");
            var response = await _enviarHttp.SendAsync(url, HttpMethod.Post, content, token);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ResponseDianNubex apiResponse = JsonConvert.DeserializeObject<ResponseDianNubex>(responseBody);

                var apiResponse2 = new ResponseSimplificadoDian
                {
                    IdCompañia = IdCompañia,
                    Factura = factura.number,
                    Message = apiResponse.message,
                    Errores = apiResponse.ResponseDian?.Envelope?.Body?.SendBillSyncResponse?.SendBillSyncResult?.ErrorMessage?.@string ?? new List<string>(),
                    Cufe = apiResponse.cufe
                };

                return apiResponse2;

            }

            return new ResponseSimplificadoDian
            {
                Message = "Algo salio Mal"
            };

        }

    }
}
