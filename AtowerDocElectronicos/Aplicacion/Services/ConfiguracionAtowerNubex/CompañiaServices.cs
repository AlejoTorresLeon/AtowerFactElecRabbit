using AtowerDocElectronico.Aplicacion.Dtos.Autenticacion;
using AtowerDocElectronico.Aplicacion.Dtos.ConfiguracionAtowerNubex;
using AtowerDocElectronico.Aplicacion.Dtos.Maestros_Parametros;
using AtowerDocElectronico.Aplicacion.Interfaces;
using AtowerDocElectronico.Aplicacion.Services.Autenticacion;
using AtowerDocElectronico.Aplicacion.Validations;
using AtowerDocElectronico.Infraestructura.Entities;
using AtowerDocElectronico.Infraestructura.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AtowerDocElectronico.Aplicacion.Services.ConfiguracionAtowerNubex
{
    public class CompañiaServices:ICompañia
    {
        private readonly IEnviarHttp _enviarHttp;
        private readonly IConfiguration _configuration;
        private readonly IAtowerDbContext _atowerDbContext;
        private readonly ILogin _login;
        private readonly INubexDbContext _nubexDbContext;
        public CompañiaServices(IEnviarHttp enviarHttp,IConfiguration configuration, IAtowerDbContext atowerDbContext, ILogin login, INubexDbContext nubexDbContext)
        {
            _enviarHttp = enviarHttp;
            _configuration = configuration;
            _atowerDbContext = atowerDbContext;
            _login = login;
            _nubexDbContext = nubexDbContext;
        }

        public async Task<ResponseGenericDtos?> CrearCompañia(CompañiaPostDto compani,int idUsuario)
        {
            var result = new ResponseGenericDtos();
            result.Errors = new List<string>();

            try
            {
                // Validar datos de entrada
                if (compani == null)
                {
                    result.Message = ("Los datos de la compañía son nulos.");
                    result.Success = false;
                    return result;
                }
                // Acceder a la conexión de base de datos a través de Entity Framework Core
                using IDbConnection dbConnection = _nubexDbContext.Database.GetDbConnection();

                // Construir la consulta SQL base
                var sql = "SELECT email FROM users where email = @Email";
                var email = await dbConnection.QueryFirstOrDefaultAsync(sql , new { Email = compani .Correo});

                var email2 = await _atowerDbContext.Compañias.FirstOrDefaultAsync(c => c.Email == compani.Correo);

                var compañia = await _atowerDbContext.Compañias.FirstOrDefaultAsync(c => c.Identificacion == compani.Identificacion.ToString());

                if (compañia != null)
                {
                    result.Message = ("La Compañia ya existe.");
                    result.Success = false;
                    return result;
                }
                if (email != null || email2 != null)
                {
                    result.Message = ("Este Email ya ah sido utilizado.");
                    result.Success = false;
                    return result;
                }

                var url = $"{_configuration["NubexApi"]}/api/ubl2.1/config/{compani.Identificacion}/{compani.DigitoVerificacion}";
                var transformedData = TrandormacionCrearCompany(compani);
                Console.WriteLine(transformedData);
                var content = new StringContent(JsonConvert.SerializeObject(transformedData), Encoding.UTF8, "application/json");
            
                var response = await _enviarHttp.SendAsync(url, HttpMethod.Post, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var data = BuildSuccessResponseCrearCompany(responseBody);


                    var userClient = await CrearUsuarioCliente(compani);

                    if (userClient == null)
                    {
                        result.Message =  "Error al crear usuario cliente en Atower.";
                        result.Success = false;
                        return result;
                    }

                    var crearCompañiaAtower = await CrearCompañiaAtower(compani,data,idUsuario,userClient.Id);


                    if (crearCompañiaAtower.Success == true)
                    {
                        result.Data = crearCompañiaAtower.Data;
                        result.Success = true;
                        result.Message = crearCompañiaAtower.Message;
                    }
                    else
                    {
                        result.Success = crearCompañiaAtower.Success;
                        result.Message = crearCompañiaAtower.Message;
                    }                    
                }
                else
                {                    

                    var responseBody = await response.Content.ReadAsStringAsync();

                    var errorResponse = BuildErrorResponse(responseBody);
                    result.Errors.AddRange(errorResponse.Details ?? new List<string>());
                    result.Message = errorResponse.Error ?? "Error al crear compañía.";
                    result.Success = false;

                }
            }
            catch (HttpRequestException ex)
            {
                result.Errors.Add($"Error al conectar con el servidor remoto. {ex}");
                // Loggear la excepción
                result.Success = false;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error interno del servidor {ex}");
                // Loggear la excepción
                result.Success = false;
            }

            return result;
        }

        private async Task<ResponseGenericDtos> CrearCompañiaAtower(CompañiaPostDto compani, CompanyResponseDTOs? nubex,int idUsuario,int idUsuarioCliente)
        {
            var result = new ResponseGenericDtos();
            result.Errors = new List<string>();
            try
            {               
                if (nubex == null)
                {
                    result.Success = false;
                    return result;
                }


                var nuevaCompañia = new Compañia
                {
                    DigitoVerificacion = compani.DigitoVerificacion.ToString(),
                    Identificacion = compani.Identificacion.ToString(),
                    RazonSocial = compani.NombreEmpresa,
                    Email = compani.Correo,
                    IdUsuarioNubex = (ulong)(nubex?.company?.user_id ?? 0),
                    IdCompanyNubex = (ulong)(nubex?.company?.id ?? 0),
                    TokenNubex = nubex?.token ?? "",
                    Habilitado = true,
                    Bloqueo = false,
                    IdUsuarioCreador = idUsuario,
                    IdUsuarioCliente = idUsuarioCliente
                };

                _atowerDbContext.Compañias.Add(nuevaCompañia);
                await _atowerDbContext.SaveChangesAsync();

                result.Data = nuevaCompañia;
                result.Success = true;
                result.Message = "Compañia creada con exito";
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Message = "Ocurrio un error al guardar en Atower";
                result.Success = false;
                result.Data = ex.Message;
                return result;
            }
         

        }

        private async Task<Usuarios?> CrearUsuarioCliente (CompañiaPostDto compani)
        {
            //var saltBytes = _login.GenerateSalt();
            //string passwordHash = _login.GeneratePasswordHash(compani.Identificacion.ToString(), Convert.FromBase64String(saltBytes));

            var userClienteNew = new CrearUsuario
            {
                Identificacion = (ulong)compani.Identificacion,
                Nombre = compani.NombreEmpresa,
                Email = compani.Correo,
                IdRol = 4,
                Password = compani.Identificacion.ToString()

            };
            var usercliente = await _login.CreateUser(userClienteNew);

            return usercliente;
        }

        private object TrandormacionCrearCompany(CompañiaPostDto companyCreationDto)
        {
            return new
            {
                type_document_identification_id = companyCreationDto.IdTipoIdentificacion,
                type_organization_id = companyCreationDto.IdTipoOrganizacion,
                type_regime_id = companyCreationDto.IdTipoRegimen,
                type_liability_id = companyCreationDto.IdTipoResponsabilidad,
                business_name = companyCreationDto.NombreEmpresa,
                merchant_registration = companyCreationDto.NumeroCamaraComercio,
                municipality_id = companyCreationDto.IdCiudad,
                address = companyCreationDto.Direccion,
                phone = companyCreationDto.Telefono,
                email = companyCreationDto.Correo
            };
        }

        private CompanyResponseDTOs? BuildSuccessResponseCrearCompany(string responseBody)
        {
            var responseObject = JsonConvert.DeserializeObject<CompanyResponseDTOs>(responseBody);

            return new CompanyResponseDTOs
            {
                success = responseObject?.success,
                message = responseObject?.message,
                password = responseObject?.password,
                token = responseObject?.token,
                company = new CompanyDTOs
                {
                    id = responseObject?.company?.id,
                    user_id = responseObject?.company?.user_id,
                    identification_number = responseObject?.company?.identification_number,
                    dv = responseObject?.company?.dv,
                    language_id = responseObject?.company?.language_id,
                    tax_id = responseObject?.company?.tax_id,
                    type_environment_id = responseObject?.company?.type_environment_id,
                    payroll_type_environment_id = responseObject?.company?.payroll_type_environment_id,
                    sd_type_environment_id = responseObject?.company?.sd_type_environment_id,
                    type_operation_id = responseObject?.company?.type_operation_id,
                    type_document_identification_id = responseObject?.company?.type_document_identification_id,
                    country_id = responseObject?.company?.country_id,
                    type_currency_id = responseObject?.company?.type_currency_id,
                    type_organization_id = responseObject?.company?.type_organization_id,
                    type_regime_id = responseObject?.company?.type_regime_id,
                    type_liability_id = responseObject?.company?.type_liability_id,
                    municipality_id = responseObject?.company?.municipality_id,
                    merchant_registration = responseObject?.company?.merchant_registration,
                    address = responseObject?.company?.address,
                    phone = responseObject?.company?.phone,
                    password = responseObject?.company?.password,
                    newpassword = responseObject?.company?.newpassword,
                    type_plan_id = responseObject?.company?.type_plan_id,
                    type_plan2_id = responseObject?.company?.type_plan2_id,
                    type_plan3_id = responseObject?.company?.type_plan3_id,
                    type_plan4_id = responseObject?.company?.type_plan4_id,
                    start_plan_date = responseObject?.company?.start_plan_date,
                    start_plan_date2 = responseObject?.company?.start_plan_date2,
                    start_plan_date3 = responseObject?.company?.start_plan_date3,
                    start_plan_date4 = responseObject?.company?.start_plan_date4,
                    absolut_start_plan_date = responseObject?.company?.absolut_start_plan_date,
                    state = responseObject?.company?.state,
                    created_at = responseObject?.company?.created_at,
                    updated_at = responseObject?.company?.updated_at,
                    user = new UserDTOs
                    {
                        id = responseObject?.company?.user?.id,
                        name = responseObject?.company?.user?.name,
                        email = responseObject?.company?.user?.email,
                        email_verified_at = responseObject?.company?.user?.email_verified_at,
                        created_at = responseObject?.company?.user?.created_at,
                        updated_at = responseObject?.company?.user?.updated_at,
                        id_administrator = responseObject?.company?.user?.id_administrator,
                        mail_host = responseObject?.company?.user?.mail_host,
                        mail_port = responseObject?.company?.user?.mail_port,
                        mail_username = responseObject?.company?.user?.mail_username,
                        mail_password = responseObject?.company?.user?.mail_password,
                        mail_encryption = responseObject?.company?.user?.mail_encryption
                    }
                }
            };
        }


        private ErrorResponseDtos BuildErrorResponse(string responseBody)
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(responseBody);
                return new ErrorResponseDtos
                {
                    Error = $"Validation error. {errorResponse.Message}",
                    Details = errorResponse?.Errors?.SelectMany(kv => kv.Value).ToList()
                };
            }
            catch (JsonException)
            {
                // Si la respuesta no puede ser deserializada como un error, se devuelve un objeto genérico.
                return new ErrorResponseDtos
                {
                    Error = "Unexpected error",
                    Details = new List<string> { responseBody }
                };
            }
        }

    }
}
